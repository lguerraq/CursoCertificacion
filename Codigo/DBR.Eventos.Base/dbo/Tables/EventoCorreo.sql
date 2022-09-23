CREATE TABLE [dbo].[EventoCorreo] (
    [IdEventoCorreo]      INT            IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT            NOT NULL,
    [Asunto]              VARCHAR (150)  NULL,
    [Origen]              VARCHAR (50)   NULL,
    [NombreOrigen]        VARCHAR (150)  NULL,
    [Mensaje]             NVARCHAR (MAX) NULL,
    [EstadoCorreo]        INT            NULL,
    [FechaEnvio]          DATETIME       NULL,
    [NumeroEnvio]         INT            NULL,
    [Estado]              BIT            NOT NULL,
    [UsuarioCreacion]     INT            NOT NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioModificacion] INT            NULL,
    [FechaModificacion]   DATETIME       NULL,
    CONSTRAINT [PK_EventoCorreo] PRIMARY KEY CLUSTERED ([IdEventoCorreo] ASC),
    CONSTRAINT [FK_EventoCorreo_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento])
);

