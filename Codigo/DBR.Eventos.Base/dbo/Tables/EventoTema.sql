CREATE TABLE [dbo].[EventoTema] (
    [IdEventoTema] INT IDENTITY (1, 1) NOT NULL,
    [IdEvento]     INT NULL,
    [TipoTema]     INT NULL,
    CONSTRAINT [PK_EventoTema] PRIMARY KEY CLUSTERED ([IdEventoTema] ASC),
    CONSTRAINT [FK_EventoTema_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento])
);

