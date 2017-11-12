using Microsoft.AspNetCore.Mvc;
using Moq;
using Spammer.Controllers;
using Spammer.Data;
using Spammer.Models;
using Spammer.Sending;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Spammer.Tests.Controllers
{
    public class SubscriptionControllerTests
    {
        [Fact]
        public void GetShouldReturnSubscriptionFromRepositoryIfMailIsInRepository()
        {
            const string testMail = "test@mail.com";
            var testTopic = new Topic { Abbreviation = "TT", Description = "Test Topic" };

            var testSubscription = new Subscription { Mail = testMail, Topics = new List<Topic> { testTopic } };

            var repositoryMock = new Mock<ISubscriptionRepository>();

            repositoryMock
                .Setup(repository => repository.GetSubscription(testMail))
                .Returns(testSubscription);

            var sendContentMock = new Mock<ISendContent>();

            var controller = new SubscriptionController(repositoryMock.Object, sendContentMock.Object);

            Subscription subscription = controller.GetSubscription(testMail);

            Assert.Equal(testMail, subscription.Mail);
            Assert.Equal(testTopic, subscription.Topics.Single());
        }

        [Fact]
        public void GetShouldReturnNewSubscriptionFromRepositoryIfMailIsNotInRepository()
        {
            const string testMail = "test@mail.com";

            var repositoryMock = new Mock<ISubscriptionRepository>();

            repositoryMock
                .Setup(repository => repository.GetSubscription(testMail))
                .Returns((Subscription)null);

            var sendContentMock = new Mock<ISendContent>();

            var controller = new SubscriptionController(repositoryMock.Object, sendContentMock.Object);

            Subscription subscription = controller.GetSubscription(testMail);

            Assert.Equal(testMail, subscription.Mail);
            Assert.Empty(subscription.Topics);
        }

        [Fact]
        public void ChangeSubscriptionShouldRemoveSubscriptionFromRepositoryAndSendConfirmationIfAllTopicsAreRemovedAndReturn204()
        {
            const string testMail = "test@mail.com";
            var testTopicA = new Topic { Abbreviation = "TA", Description = "Test Topic A" };
            var testTopicB = new Topic { Abbreviation = "TB", Description = "Test Topic A" };

            var testSubscription = new Subscription { Mail = testMail, Topics = new List<Topic> { testTopicA, testTopicB } };

            var repositoryMock = new Mock<ISubscriptionRepository>();

            repositoryMock
                .Setup(repository => repository.GetSubscription(testMail))
                .Returns(testSubscription);

            var sendContentMock = new Mock<ISendContent>();

            var controller = new SubscriptionController(repositoryMock.Object, sendContentMock.Object);

            IActionResult result = controller.ChangeSubscription(testMail, Enumerable.Empty<string>().ToArray());
            var objectResult = result as NoContentResult;

            Assert.NotNull(objectResult);

            repositoryMock.Verify(repository => repository.RemoveSubscription(It.IsAny<Subscription>()), Times.Once);
            sendContentMock.Verify(sendContent => sendContent.Send(testMail, It.IsAny<Content>()), Times.Once);
        }

        [Fact]
        public void ChangeSubscriptionShouldNotRemoveSubscriptionFromRepositoryAndNotSendConfirmationIfNotAllTopicsAreRemovedAndReturn201()
        {
            const string testMail = "test@mail.com";
            var testTopicA = new Topic { Abbreviation = "TA", Description = "Test Topic A" };
            var testTopicB = new Topic { Abbreviation = "TB", Description = "Test Topic A" };

            var testSubscription = new Subscription { Mail = testMail, Topics = new List<Topic> { testTopicA, testTopicB } };

            var repositoryMock = new Mock<ISubscriptionRepository>();

            repositoryMock
                .Setup(repository => repository.GetSubscription(testMail))
                .Returns(testSubscription);

            repositoryMock
                .Setup(repository => repository.GetTopics())
                .Returns(new List<Topic> { testTopicA, testTopicB });

            var sendContentMock = new Mock<ISendContent>();

            var controller = new SubscriptionController(repositoryMock.Object, sendContentMock.Object);

            IActionResult result = controller.ChangeSubscription(testMail, new string[] { testTopicA.Abbreviation });
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(201, objectResult.StatusCode.Value);

            repositoryMock.Verify(repository => repository.RemoveSubscription(It.IsAny<Subscription>()), Times.Never);
            sendContentMock.Verify(sendContent => sendContent.Send(testMail, It.IsAny<Content>()), Times.Never);
        }
    }
}