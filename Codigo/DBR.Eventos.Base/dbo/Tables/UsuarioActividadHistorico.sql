CREATE TABLE [dbo].[UsuarioActividadHistorico] (
    [IdUsuarioActividad] INT      IDENTITY (1, 1) NOT NULL,
    [IdUsuario]          INT      NULL,
    [FechaCreacion]      DATETIME NULL
);

