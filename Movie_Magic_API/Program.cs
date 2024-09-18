using Application_Layer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<CORE.Interface.IBookingRepository, Infra_Structure_Layer.Repositories.BookingRepository>();
builder.Services.AddScoped<CORE.Interface.IMovieRepository, Infra_Structure_Layer.Repositories.MovieRepository>();
builder.Services.AddScoped<CORE.Interface.IMovieShowsRepository, Infra_Structure_Layer.Repositories.MovieShowsRepository>();
builder.Services.AddScoped(typeof(CORE.Interface.IRepository<>), typeof(Infra_Structure_Layer.Repositories.GenericRepository<>));
builder.Services.AddScoped(typeof(GenericService<>));
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<MovieShowsService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();












// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
