using ReadFromDatabase;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IPersonRepository>(new PersonRepositoryDB());
builder.Services.AddSingleton<ITitleRepository>(new TitleRepositoryDB());
builder.Services.AddSingleton<IGenreRepository>(new GenreRepositoryDB());
builder.Services.AddSingleton<IProfessionRepository>(new ProfessionRepositoryDB());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("def", opts =>
    {
        opts.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); 
});
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("def");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
