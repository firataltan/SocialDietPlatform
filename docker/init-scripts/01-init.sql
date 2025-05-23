-- Create database
CREATE DATABASE SocialDietPlatformDb;
GO

USE SocialDietPlatformDb;
GO

-- Create schema
CREATE SCHEMA socialdiet;
GO

-- Create tables
CREATE TABLE socialdiet.Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    ProfilePictureUrl NVARCHAR(500),
    Bio NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'Active',
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE TABLE socialdiet.Recipes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Ingredients NVARCHAR(MAX) NOT NULL,
    Instructions NVARCHAR(MAX) NOT NULL,
    PrepTimeMinutes INT NOT NULL,
    CookTimeMinutes INT NOT NULL,
    Servings INT NOT NULL,
    Difficulty NVARCHAR(20) NOT NULL,
    Category NVARCHAR(20) NOT NULL,
    ImageUrl NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Recipes_Users FOREIGN KEY (UserId) REFERENCES socialdiet.Users(Id) ON DELETE CASCADE
);

CREATE TABLE socialdiet.Comments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    RecipeId UNIQUEIDENTIFIER NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES socialdiet.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Comments_Recipes FOREIGN KEY (RecipeId) REFERENCES socialdiet.Recipes(Id) ON DELETE CASCADE
);

CREATE TABLE socialdiet.Likes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    RecipeId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Likes_Users FOREIGN KEY (UserId) REFERENCES socialdiet.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Likes_Recipes FOREIGN KEY (RecipeId) REFERENCES socialdiet.Recipes(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Likes_UserRecipe UNIQUE (UserId, RecipeId)
);

CREATE TABLE socialdiet.Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(20) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Unread',
    AdditionalData NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES socialdiet.Users(Id) ON DELETE CASCADE
);

-- Create indexes
CREATE INDEX IX_Users_Email ON socialdiet.Users(Email);
CREATE INDEX IX_Users_Username ON socialdiet.Users(Username);
CREATE INDEX IX_Recipes_UserId ON socialdiet.Recipes(UserId);
CREATE INDEX IX_Recipes_Category ON socialdiet.Recipes(Category);
CREATE INDEX IX_Comments_RecipeId ON socialdiet.Comments(RecipeId);
CREATE INDEX IX_Likes_RecipeId ON socialdiet.Likes(RecipeId);
CREATE INDEX IX_Notifications_UserId ON socialdiet.Notifications(UserId);
CREATE INDEX IX_Notifications_Status ON socialdiet.Notifications(Status);

-- Create trigger for UpdatedAt
CREATE TRIGGER TR_Users_UpdateUpdatedAt
ON socialdiet.Users
AFTER UPDATE
AS
BEGIN
    UPDATE socialdiet.Users
    SET UpdatedAt = GETUTCDATE()
    FROM socialdiet.Users u
    INNER JOIN inserted i ON u.Id = i.Id;
END;
GO

CREATE TRIGGER TR_Recipes_UpdateUpdatedAt
ON socialdiet.Recipes
AFTER UPDATE
AS
BEGIN
    UPDATE socialdiet.Recipes
    SET UpdatedAt = GETUTCDATE()
    FROM socialdiet.Recipes r
    INNER JOIN inserted i ON r.Id = i.Id;
END;
GO

CREATE TRIGGER TR_Comments_UpdateUpdatedAt
ON socialdiet.Comments
AFTER UPDATE
AS
BEGIN
    UPDATE socialdiet.Comments
    SET UpdatedAt = GETUTCDATE()
    FROM socialdiet.Comments c
    INNER JOIN inserted i ON c.Id = i.Id;
END;
GO 