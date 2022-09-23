CREATE TABLE [dbo].[OpcionUsuarioTipo] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [IdOpcion]            INT      NOT NULL,
    [IdUsuarioTipo]       INT      NOT NULL,
    [Estado]              BIT      NULL,
    [FechaCreacion]       DATETIME NULL,
    [UsuarioCreacion]     INT      NULL,
    [FechaModificacion]   DATETIME NULL,
    [UsuarioModificacion] INT      NULL,
    CONSTRAINT [PK_OpcionUsuarioTipo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OpcionUsuarioTipo_Opcion] FOREIGN KEY ([IdOpcion]) REFERENCES [dbo].[Opcion] ([IdOpcion]),
    CONSTRAINT [FK_OpcionUsuarioTipo_UsuarioTipo] FOREIGN KEY ([IdUsuarioTipo]) REFERENCES [dbo].[UsuarioTipo] ([IdUsuarioTipo])
);

