using TodoListWebApi.Application.Common.Interfaces;
using TodoListWebApi.Application.Common.Models;
using TodoListWebApi.Application.TodoLists.Queries.GetTodos;

namespace TodoListWebApi.Application.TodoItems.Queries.GetTodoItem;

public record GetTodoItemQuery : IRequest<TodoItemDto>
{
    public int Id { get; init; }
}

public class GetTodoItemQueryHandler : IRequestHandler<GetTodoItemQuery, TodoItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(GetTodoItemQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _context.TodoItems
            .Where(x => x.Id == request.Id)
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken);

        return result;
    }
}
