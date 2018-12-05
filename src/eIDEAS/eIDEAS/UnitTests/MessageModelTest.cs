using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eIDEAS.UnitTests
{
    public class MessageModelTest
    {
        ApplicationDbContext _context;

        public MessageModelTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
        }

        [Fact]
        public async void TestMessageModel()
        {
            //Create new Success Story
            var successStory = new Message
            {
                AuthorID = new Guid(),
                Title = "Fake title",
                Text = "This is a fake success story",
                DateCreated = DateTime.UtcNow,
                MessageType = Models.Enums.MessageEnum.SuccessStory
            };

            //Add to db
            _context.Message.Add(successStory);

            //Save changes
            await _context.SaveChangesAsync();

            //Confirm it was added to the db successfully
            IEnumerable<Message> messageList = _context.Message.Where(row => row.ID == successStory.ID).ToList();
            Assert.Equal(messageList.ElementAt(0), successStory);

            //Update model in the db
            successStory.Title = "This is the updated title";
            _context.Message.Update(successStory);

            //Save changes
            await _context.SaveChangesAsync();

            //Query the db again to make sure update went through
            messageList = _context.Message.Where(row => row.ID == successStory.ID).ToList();

            //Make sure model was in db
            Assert.Equal(messageList.ElementAt(0), successStory);

            //Delete the fake story
            _context.Message.Remove(successStory);

            //Save changes
            await _context.SaveChangesAsync();

            //Make sure it does not exist in DB anymore
            Assert.Empty(_context.Message.Where(row => row.ID == successStory.ID).ToList());
        }
    }
}
