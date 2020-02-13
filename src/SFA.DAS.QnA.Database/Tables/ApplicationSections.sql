CREATE TABLE [ApplicationSections]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[SequenceNo] [int] NOT NULL,
	[SectionNo] [int] NOT NULL,
	[QnAData] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[LinkTitle] [nvarchar](250) NOT NULL,
	[DisplayType] [nvarchar](50) NOT NULL,
   [SequenceId] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_ApplicationSections] PRIMARY KEY (	[Id] )
) 
GO

ALTER TABLE [ApplicationSections] ADD  CONSTRAINT [DF_ApplicationSequences_Id]  DEFAULT (newid()) FOR [Id]
GO

CREATE INDEX [IX_ApplicationSections_ApplicationId] ON [ApplicationSections]   (  [ApplicationId]  )
GO

CREATE  INDEX [IX_ApplicationSections_SequenceId] ON [ApplicationSections]   (  [SequenceId]  )
GO