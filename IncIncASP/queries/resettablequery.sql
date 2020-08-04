DROP TABLE [dbo].[tblEntries];
CREATE TABLE [dbo].[tblEntries] (
    [EntryId]   INT             IDENTITY (1, 1) NOT NULL,
    [FirstName] TEXT            NOT NULL,
    [LastName]  TEXT            NOT NULL,
    [Messages]  INT             NOT NULL,
    [Pay]       DECIMAL (18, 2) NOT NULL,
    [EntryDate] DATETIME        NOT NULL,
	[WorkerType] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([EntryId] ASC)
);
INSERT INTO [dbo].[tblEntries] (EntryId, FirstName, LastName, Messages, Pay, EntryDate, WorkerType)
VALUES (1, "Pyle", "Ghapman", 500, 11.0, CURRENT_TIMESTAMP, 0);
INSERT INTO [dbo].[tblEntries] (EntryId, FirstName, LastName, Messages, Pay, EntryDate, WorkerType)
VALUES (1, "Myle", "Mapman", 600, 13.20, CURRENT_TIMESTAMP, 0);
INSERT INTO [dbo].[tblEntries] (EntryId, FirstName, LastName, Messages, Pay, EntryDate, WorkerType)
VALUES (1, "Tyle", "Tapman", 19000, 665.0, CURRENT_TIMESTAMP, 0);