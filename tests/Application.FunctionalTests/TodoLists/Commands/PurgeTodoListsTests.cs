using TodoListWebApi.Application.Common.Exceptions;
using TodoListWebApi.Application.Common.Security;
using TodoListWebApi.Application.TodoLists.Commands.CreateTodoList;
using TodoListWebApi.Application.TodoLists.Commands.PurgeTodoLists;
using TodoListWebApi.Domain.Entities;

namespace TodoListWebApi.Application.FunctionalTests.TodoLists.Commands;

using static Testing;

public class PurgeTodoListsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteAllLists()
    {
        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #1"
        });

        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #2"
        });

        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #3"
        });

        await SendAsync(new PurgeTodoListsCommand());

        var count = await CountAsync<TodoList>();

        count.Should().Be(0);
    }
}
