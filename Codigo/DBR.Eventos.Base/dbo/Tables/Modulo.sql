CREATE TABLE [dbo].[Modulo] (
    [IdModulo]            INT              IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT              NULL,
    [Nombre]              VARCHAR (100)    NULL,
    [Descripcion]         VARCHAR (MAX)    NULL,
    [Expositor]           VARCHAR (150)    NULL,
    [Horas]               INT              NULL,
    [Peso]                INT              NULL,
    [Estado]              BIT              NULL,
    [rowid]               UNIQUEIDENTIFIER NULL,
    [UsuarioCreacion]     INT              NOT NULL,
    [FechaCreacion]       DATETIME         NOT NULL,
    [UsuarioModificacion] INT              NULL,
    [FechaModificacion]   DATETIME         NULL,
    CONSTRAINT [PK_Modulo] PRIMARY KEY CLUSTERED ([IdModulo] ASC),
    CONSTRAINT [FK_Modulo_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento])
);



