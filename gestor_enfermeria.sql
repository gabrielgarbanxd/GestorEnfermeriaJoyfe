create database if not exists gestor_enfemeria;
use gestor_enfemeria;

-- *************************************************************************
-- *                                                                       *
-- *                             TABLE DEFINITIONS                        *
-- *                                                                       *
-- *************************************************************************

-- Users table
DROP TABLE IF EXISTS `gestor_enfemeria`.`users`;

CREATE TABLE `gestor_enfemeria`.`users` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `password` VARCHAR(255) NOT NULL,
  `last_name` VARCHAR(50) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE);



-- *************************************************************************
-- *                                                                       *
-- *                           STORED PROCEDURES                          *
-- *                                                                       *
-- *************************************************************************

-- ==================================
-- ========>>    USERS    <<=========
-- ==================================

-- //===>> Create user procedure <<===//
DROP PROCEDURE IF EXISTS `CreateUserProcedure`;

DELIMITER $$

CREATE PROCEDURE `CreateUserProcedure`(
    IN p_name VARCHAR(50),
    IN p_password VARCHAR(255),
    IN p_last_name VARCHAR(50),
    IN p_email VARCHAR(255),
    OUT p_Result INT
)
PRO : BEGIN

    -- Verificar si el email ya existe
    IF EXISTS (SELECT * FROM users WHERE email = p_email) THEN
        SET p_Result = -1; -- Código de error para email duplicado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -2; -- Código de error para nombre nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el apellido no sea nulo
    IF p_last_name IS NULL THEN
        SET p_Result = -3; -- Código de error para apellido nulo
        LEAVE PRO;
    END IF;

    -- Insertar nuevo usuario
    INSERT INTO users(name, password, last_name, email)
    VALUES(p_name, p_password, p_last_name, p_email);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID del usuario insertado
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;






-- *************************************************************************
-- *                                                                       *
-- *                                 VIEWS                               *
-- *                                                                       *
-- *************************************************************************




-- *************************************************************************
-- *                                                                       *
-- *                               FUNCTIONS                             *
-- *                                                                       *
-- *************************************************************************
