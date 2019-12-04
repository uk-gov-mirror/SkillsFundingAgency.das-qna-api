﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.QnA.Api.Types;
using SFA.DAS.QnA.Api.Types.Page;
using SFA.DAS.QnA.Application.Commands.SetPageAnswers;
using SFA.DAS.QnA.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.QnA.Application.Commands.SkipPage
{
    public class SkipPageHandler : SetAnswersBase, IRequestHandler<SkipPageRequest, HandlerResponse<SkipPageResponse>>
    {

        public SkipPageHandler(QnaDataContext dataContext) : base(dataContext)
        {
        }

        public async Task<HandlerResponse<SkipPageResponse>> Handle(SkipPageRequest request, CancellationToken cancellationToken)
        {
            var application = await _dataContext.Applications.AsNoTracking().SingleOrDefaultAsync(app => app.Id == request.ApplicationId, cancellationToken: cancellationToken);
            if (application is null) return new HandlerResponse<SkipPageResponse>(false, "Application does not exist");

            var section = await _dataContext.ApplicationSections.AsNoTracking().SingleOrDefaultAsync(sec => sec.Id == request.SectionId && sec.ApplicationId == request.ApplicationId, cancellationToken);
            if (section is null) return new HandlerResponse<SkipPageResponse>(false, "Section does not exist");

            var qnaData = new QnAData(section.QnAData);
            var page = qnaData.Pages.SingleOrDefault(p => p.PageId == request.PageId);
            if (page is null) return new HandlerResponse<SkipPageResponse>(false, "Page does not exist");

            try
            {
                var nextAction = GetNextActionForPage(section.Id, page.PageId);
                var checkboxListAllNexts = GetCheckboxListMatchingNextActionsForPage(section.Id, page.PageId);
                var answers = page.PageOfAnswers?[0].Answers;

                SetStatusOfNextPagesBasedOnAnswer(section.Id, page.PageId, answers, nextAction, checkboxListAllNexts);

                return new HandlerResponse<SkipPageResponse>(new SkipPageResponse(nextAction.Action, nextAction.ReturnId));
            }
            catch (ApplicationException)
            {
                if (page.Next is null || !page.Next.Any())
                {
                    return new HandlerResponse<SkipPageResponse>(new SkipPageResponse());
                }
                else
                {
                    return new HandlerResponse<SkipPageResponse>(false, "Cannot find a matching 'Next' instruction");
                }
            }
        }
    }
}
