CREATE TABLE [dbo].[Docente] (
    [IdDocuente]          INT              IDENTITY (1, 1) NOT NULL,
    [Nombre]              VARCHAR (150)    NULL,
    [NombreFoto]          VARCHAR (150)    NULL,
    [Profesion]           VARCHAR (250)    NULL,
    [Especialista]        VARCHAR (250)    NULL,
    [Detalle]             VARCHAR (MAX)    NULL,
    [Estado]              BIT              CONSTRAINT [DF_Docente_Estado] DEFAULT ((1)) NOT NULL,
    [rowid]               UNIQUEIDENTIFIER CONSTRAINT [DF_Docente_rowid] DEFAULT (newid()) NOT NULL,
    [UsuarioCreacion]     INT              NOT NULL,
    [FechaCreacion]       DATETIME         CONSTRAINT [DF_Docente_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioModificacion] INT              NULL,
    [FechaModificacion]   DATETIME         NULL,
    CONSTRAINT [PK_Docente] PRIMARY KEY CLUSTERED ([IdDocuente] ASC)
);

