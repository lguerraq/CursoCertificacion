CREATE TABLE [dbo].[Suceso] (
    [IdSuceso]            INT              IDENTITY (1, 1) NOT NULL,
    [NombreSuceso]        VARCHAR (250)    NULL,
    [Descripcion]         VARCHAR (1000)   NULL,
    [Fecha]               DATETIME         NULL,
    [ImagenSuceso]        VARCHAR (150)    NULL,
    [Horas]               INT              NULL,
    [Activo]              BIT              NULL,
    [Estado]              BIT              NULL,
    [rowid]               UNIQUEIDENTIFIER NULL,
    [UsuarioCreacion]     INT              NOT NULL,
    [FechaCreacion]       DATETIME         NOT NULL,
    [UsuarioModificacion] INT              NULL,
    [FechaModificacion]   DATETIME         NULL,
    CONSTRAINT [PK_Suceso] PRIMARY KEY CLUSTERED ([IdSuceso] ASC)
);

