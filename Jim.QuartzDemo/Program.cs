using Jim.Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<Jim.QuartzDemo.Service.ITestService, Jim.QuartzDemo.Service.TestService>();

// 注册定时器
builder.Services.AddJimQuartz();

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


var testService = app.Services.GetRequiredService<Jim.QuartzDemo.Service.ITestService>();
// 设置立即执行的任务
app.UseJimQuart(testService.TestLog());

app.Run();
