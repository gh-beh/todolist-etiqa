using TodoListWebApi.Application.TodoItems.Commands.CreateTodoItem;
using TodoListWebApi.Application.TodoItems.Commands.UpdateTodoItem;
using TodoListWebApi.Application.TodoItems.Commands.UpdateTodoItemDetail;
using TodoListWebApi.Application.TodoLists.Commands.CreateTodoList;
using TodoListWebApi.Domain.Entities;
using TodoListWebApi.Domain.Enums;

namespace TodoListWebApi.Application.FunctionalTests.TodoItems.Commands;

using static Testing;

public class UpdateTodoItemDetailTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new UpdateTodoItemCommand { Id = 99, Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        var command = new UpdateTodoItemDetailCommand
        {
            Id = itemId,
            ListId = listId,
            Note = "This is the note.",
            Priority = PriorityLevel.High
        };

        await SendAsync(command);

        var item = await FindAsync<TodoItem>(itemId);

        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Note.Should().Be(command.Note);
        item.Priority.Should().Be(command.Priority);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
