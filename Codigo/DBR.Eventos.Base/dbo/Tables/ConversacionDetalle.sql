CREATE TABLE [dbo].[ConversacionDetalle] (
    [IdConversacionDetalle] INT           IDENTITY (1, 1) NOT NULL,
    [IdConversacion]        INT           NULL,
    [Descripcion]           VARCHAR (500) NULL,
    [FechaHora]             DATETIME      NULL,
    CONSTRAINT [PK_ConversacionDetalle] PRIMARY KEY CLUSTERED ([IdConversacionDetalle] ASC),
    CONSTRAINT [FK_ConversacionDetalle_Conversacion] FOREIGN KEY ([IdConversacion]) REFERENCES [dbo].[Conversacion] ([IdConversacion])
);

