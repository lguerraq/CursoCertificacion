CREATE TABLE [dbo].[Opcion] (
    [IdOpcion]            INT           IDENTITY (1, 1) NOT NULL,
    [IdPadre]             INT           NULL,
    [Icono]               VARCHAR (50)  NULL,
    [Descipcion]          VARCHAR (135) NULL,
    [UrlDescripcion]      VARCHAR (100) NULL,
    [Orden]               INT           NULL,
    [Estado]              BIT           NULL,
    [FechaCreacion]       DATETIME      NULL,
    [UsuarioCreacion]     INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] INT           NULL,
    CONSTRAINT [PK_Opcion] PRIMARY KEY CLUSTERED ([IdOpcion] ASC)
);

