﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.QnA.Api.Infrastructure;
using SFA.DAS.QnA.Application.Commands.CreateSnapshot;

namespace SFA.DAS.QnA.Api.Controllers
{
    [Route("/applications")]
    [Produces("application/json")]
    [ApiController]
    public class ApplicationSnapshotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationSnapshotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Creates a snapshot of the requested application. Used for RoATP
        /// </summary>
        /// <param name="applicationId">The Id of the application which is to be snapshot</param>
        /// <returns>The newly created application's Id</returns>
        /// <response code="201">Returns the newly created application's Id</response>
        /// <response code="404">If the requested application could not be found</response>
        [HttpPost("{applicationId}/snapshot")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Guid>> CreateApplicationSnapshot(Guid applicationId)
        {
            var newSnapshot = await _mediator.Send(new CreateSnapshotRequest(applicationId));

            if (!newSnapshot.Success) return NotFound(new NotFoundError(newSnapshot.Message));

            return Ok(new { newSnapshot.Value.ApplicationId });
        }
    }
}

