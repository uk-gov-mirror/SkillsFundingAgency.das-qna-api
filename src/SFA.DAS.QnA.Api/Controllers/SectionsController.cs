using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Qna.Api.Types;
using SFA.DAS.QnA.Application.Queries.Sequences.GetCurrentSequence;

namespace SFA.DAS.QnA.Api.Controllers
{
    [Route("applications")]
    [Produces("application/json")]
    public class SectionsController : Controller
    {
        private readonly IMediator _mediator;

        public SectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Returns the Sequence's Sections
        /// </summary>
        /// <returns>The Sequence's Sections</returns>
        /// <response code="200">Returns the Sequence's Sections</response>
        /// <response code="204">If there are no Sections for the given SequenceId</response>
        /// <response code="404">If the ApplicationId or SequenceId are invalid</response>
        [HttpGet("{applicationId}/sequences/{sequenceId}/current")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Section>>> GetSequenceSections(Guid applicationId, Guid sequenceId)
        {
            var sectionsResponse = await _mediator.Send(new GetSequenceSectionsRequest(applicationId, sequenceId), CancellationToken.None);
            if (!sectionsResponse.Success) return NotFound();
            if (sectionsResponse.Value == null) return NoContent();

            return sectionsResponse.Value;
        }
    }
}