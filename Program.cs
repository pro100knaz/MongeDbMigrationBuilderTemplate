using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;

// Пример коллекций (их нужно будет заменить на реальные коллекции из MongoDB)
ICollection<PersonalDashboardDocument> currentCollection = new List<PersonalDashboardDocument>();
ICollection<MigrationVersionDocument> versionCollection = new List<MigrationVersionDocument>();

// Пример логгера (обычно он предоставляется через DI)
ILogger<PersonalDashboardDocument> logger = LoggerFactory.Create(builder =>
{
	
}).CreateLogger<PersonalDashboardDocument>();

// Создание экземпляра MigrationBuilder
var migrationBuilder = MigrationBuilder<PersonalDashboardDocument, MigrationVersionDocument>.CreateBuilder(
	currentCollection,
	versionCollection,
	logger
);


migrationBuilder.CreateMigration("v1.0", "Initial migration")
	.AddProperty("Name", "John Doe")
	.UpdatePropertyName("OldName", "NewName")
	.SaveChanges()
	.CreateMigration("sdad","dawd");




public interface IMigrationBuilder<TDocument, TVersion>
{
	IMigrationStepBuilder<TDocument, TVersion> CreateMigration(string version, string description);
}

public interface IMigrationStepBuilder<TDocument, TVersion>
{
	IMigrationStepBuilder<TDocument, TVersion> AddProperty(string propertyName, object value);
	IMigrationStepBuilder<TDocument, TVersion> UpdatePropertyName(string oldName, string newName);
	IMigrationBuilder<TDocument, TVersion> SaveChanges();
}

public class MigrationBuilder<TDocument, TVersion> : IMigrationBuilder<TDocument, TVersion>, IMigrationStepBuilder<TDocument, TVersion>
{
	private readonly ICollection<TDocument> _currentCollection;
	private readonly ICollection<TVersion> _versionCollection;
	private readonly ILogger<TDocument> _logger;

	private string _currentVersion;
	private string _currentDescription;

	public MigrationBuilder(
		ICollection<TDocument> currentCollection,
		ICollection<TVersion> versionCollection,
		ILogger<TDocument> logger)
	{
		_currentCollection = currentCollection;
		_versionCollection = versionCollection;
		_logger = logger;
	}

	public static IMigrationBuilder<TDocument, TVersion> CreateBuilder(
			ICollection<TDocument> currentCollection,
			ICollection<TVersion> versionCollection,
			ILogger<TDocument> logger)
	{

		return new MigrationBuilder<TDocument, TVersion>(currentCollection, versionCollection, logger);
	}
	public IMigrationStepBuilder<TDocument, TVersion> CreateMigration(string version, string description)
	{
		_currentVersion = version;
		_currentDescription = description;
		_logger.LogInformation($"Migration started: {version} - {description}");

		// Логика подготовки к миграции, если необходимо

		return this;
	}

	public IMigrationStepBuilder<TDocument, TVersion> AddProperty(string propertyName, object value)
	{
		_logger.LogInformation($"Adding property: {propertyName} = {value}");

		// Логика добавления свойства в коллекцию миграции

		return this;
	}

	public IMigrationStepBuilder<TDocument, TVersion> UpdatePropertyName(string oldName, string newName)
	{
		_logger.LogInformation($"Updating property name: {oldName} to {newName}");

		// Логика обновления имени свойства в коллекции миграции

		return this;
	}

	public IMigrationBuilder<TDocument, TVersion> SaveChanges()
	{
		_logger.LogInformation($"Saving migration: {_currentVersion}");

		// Логика сохранения миграции в коллекцию

		// Сброс текущего состояния
		_currentVersion = null;
		_currentDescription = null;

		return this;
	}
}


public class PersonalDashboardDocument 
{
	public string Id { get; set; } // Уникальный идентификатор документа
	public string Name { get; set; } // Имя пользователя или панели
	public string Email { get; set; } // Email пользователя
	public int Age { get; set; } // Возраст пользователя
	public DateTime CreatedAt { get; set; } // Дата создания документа

	public PersonalDashboardDocument(string name, string email, int age)
	{
		Id = Guid.NewGuid().ToString(); // Генерация уникального идентификатора
		Name = name;
		Email = email;
		Age = age;
		CreatedAt = DateTime.UtcNow;
	}
}

public class MigrationVersionDocument
{
	public string Version { get; set; } // Версия миграции
	public string Description { get; set; } // Описание изменений в миграции
	public DateTime AppliedAt { get; set; } // Дата применения миграции

	public MigrationVersionDocument(string version, string description)
	{
		Version = version;
		Description = description;
		AppliedAt = DateTime.UtcNow;
	}
}