﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.QnA.Api.Types.Page;
using SFA.DAS.QnA.Application.Validators;

namespace SFA.DAS.QnA.Application.UnitTests.Validators.MaxLengthValidatorTests
{
    [TestFixture]
    public class When_Validate_Called
    {
        [TestCase("", "25", true)]
        [TestCase("Mary had a little lamb", "25", true)]
        [TestCase("    Mary had a little lamb", "25", true)]
        [TestCase("Mary had a little lamb, its fleece was white as snow", "25", false)]
        [TestCase("   Mary had a little lamb, its fleece was white as snow", "25", false)]
        public void Then_correct_errors_are_returned(string input, string maxLength, bool isValid)
        {
            var validator = new MaxLengthValidator
            {
                ValidationDefinition = new ValidationDefinition()
                {
                    ErrorMessage = "Length exceeded",
                    Name = "MaxLength",
                    Value = maxLength
                }
            };

            var question = new Question { QuestionId = "Q1" };
            var errors = validator.Validate(question, new Answer { Value = input, QuestionId = question.QuestionId });

            (errors.Count is 0).Should().Be(isValid);
        }
    }
}
