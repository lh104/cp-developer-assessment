using AutoMapper;

namespace TodoList.Api.Mapping
{
    public static class Bootstrapper
    {
        internal static IMapper Mapper { get; private set; }

        public static void Bootstrap() {
            var config = new MapperConfiguration( cfg => {
                cfg.AddProfile<TodoItemProfile>();
            } );

            config.CompileMappings();
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }
    }
}
