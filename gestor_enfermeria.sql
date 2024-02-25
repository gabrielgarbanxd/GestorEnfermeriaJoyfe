create database if not exists gestor_enfermeria default character set utf8mb4 collate utf8mb4_unicode_ci;
use gestor_enfermeria;

-- *************************************************************************
-- *                                                                       *
-- *                             TABLE DEFINITIONS                        *
-- *                                                                       *
-- *************************************************************************


-- Drop tables if they exist
DROP TABLE IF EXISTS `gestor_enfermeria`.`users`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`visits`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`patients`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`calendar`;

-- Users table
CREATE TABLE `gestor_enfermeria`.`users` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `password` VARCHAR(255) NOT NULL,
    `last_name` VARCHAR(50) NOT NULL,
    `email` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
    UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE);
    

-- Patients table
CREATE TABLE `gestor_enfermeria`.`patients` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `last_name` VARCHAR(50) NOT NULL,
    `last_name2` VARCHAR(50) NOT NULL,
    `course` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE);

-- Visits table
CREATE TABLE `gestor_enfermeria`.`visits` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `type` ENUM('Agudo', 'Crónico') NOT NULL,
    `classification` VARCHAR(255) NOT NULL,
    `description` TEXT NOT NULL,
    `comunicated` INT UNSIGNED NOT NULL,
    `derived` INT UNSIGNED NOT NULL,
    `trauma_type` ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES') NULL,
    `place` ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS') NULL,
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `patient_id` INT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
    INDEX `fk_patient_idx` (`patient_id` ASC) VISIBLE,
    CONSTRAINT `fk_patient`
    FOREIGN KEY (`patient_id`)
    REFERENCES `gestor_enfermeria`.`patients` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;



-- Calendar table
CREATE TABLE `gestor_enfermeria`.`calendar` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `date` DATE NOT NULL,
    `year` YEAR AS (YEAR(`date`)),
    `quarter` TINYINT AS (QUARTER(`date`)),
    `month` TINYINT AS (MONTH(`date`)),
    `day` TINYINT AS (DAY(`date`)),
    `task` TEXT NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE
);


-- *************************************************************************
-- *                                                                       *
-- *                           STORED PROCEDURES                          *
-- *                                                                       *
-- *************************************************************************

-- ==================================
-- ========>>    USERS    <<=========
-- ==================================

-- //===>> GetAllUsersProcedure users procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllUsersProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllUsersProcedure`()
PRO : BEGIN

    -- Obtener todos los usuarios
    SELECT * FROM users;

END$$

DELIMITER ;

-- //===>> GetAllUsersPaginatedProcedure users procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllUsersPaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllUsersPaginatedProcedure`(
    In p_per_page INT,
    In p_page INT,
    OUT p_Result INT
)
PRO : BEGIN

	DECLARE p_offset INT;

    -- Verificar que el número de página sea mayor a 0
    IF p_page < 1 THEN
        SET p_Result = -1; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el número de registros por página sea mayor a 0
    IF p_per_page < 1 THEN
        SET p_Result = -2; -- Código de error para número de registros por página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Calcular el número de registros a omitir
    SET p_offset = (p_page - 1) * p_per_page;

    -- Obtener el número total de registros
    SELECT COUNT(*) INTO @total_records FROM users;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM users LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetUserByIdProcedure users procedure <<===//
DROP PROCEDURE IF EXISTS `GetUserByIdProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetUserByIdProcedure`(
    IN p_id INT
)
PRO : BEGIN

    -- Obtener el usuario por ID
    SELECT * FROM users WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> GetUserByEmailProcedure users procedure <<===//
DROP PROCEDURE IF EXISTS `GetUserByEmailProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetUserByEmailProcedure`(
    IN p_email VARCHAR(255)
)

PRO : BEGIN

    -- Obtener el usuario por email
    SELECT * FROM users WHERE email = p_email;

END$$

DELIMITER ;


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

-- //===>> Update user procedure <<===//
DROP PROCEDURE IF EXISTS `UpdateUserProcedure`;

DELIMITER $$

CREATE PROCEDURE `UpdateUserProcedure`(
    IN p_id INT,
    IN p_name VARCHAR(50),
    IN p_password VARCHAR(255),
    IN p_last_name VARCHAR(50),
    IN p_email VARCHAR(255),
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT * FROM users WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para usuario no encontrado
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

    -- Verificar si el email ya existe
    IF EXISTS (SELECT * FROM users WHERE email = p_email AND id != p_id) THEN
        SET p_Result = -4; -- Código de error para email duplicado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Actualizar usuario
    UPDATE users
    SET name = p_name, password = p_password, last_name = p_last_name, email = p_email
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Delete user procedure <<===//
DROP PROCEDURE IF EXISTS `DeleteUserProcedure`;

DELIMITER $$

CREATE PROCEDURE `DeleteUserProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT * FROM users WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para usuario no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar usuario
    DELETE FROM users WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM users WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;





-- ==================================
-- ========>>  PATIENTS   <<=========
-- ==================================

-- //===>> GetAllPatientsProcedure patients procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllPatientsProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllPatientsProcedure`()
PRO : BEGIN

    -- Obtener todos los pacientes
    SELECT * FROM patients;

END$$

DELIMITER ;


-- //===>> GetAllPatientsPaginatedProcedure patients procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllPatientsPaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllPatientsPaginatedProcedure`(
    In p_per_page INT,
    In p_page INT,
    OUT p_Result INT
)
PRO : BEGIN

    DECLARE p_offset INT;

    -- Verificar que el número de página sea mayor a 0
    IF p_page < 1 THEN
        SET p_Result = -1; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el número de registros por página sea mayor a 0
    IF p_per_page < 1 THEN
        SET p_Result = -2; -- Código de error para número de registros por página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Calcular el número de registros a omitir
    SET p_offset = (p_page - 1) * p_per_page;

    -- Obtener el número total de registros
    SELECT COUNT(*) INTO @total_records FROM patients;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM patients LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetPatientByIdProcedure patients procedure <<===//
DROP PROCEDURE IF EXISTS `GetPatientByIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetPatientByIdProcedure`(
    IN p_id INT
)
PRO : BEGIN

    -- Obtener el paciente por ID
    SELECT * FROM patients WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> Create patient procedure <<===//
DROP PROCEDURE IF EXISTS `CreatePatientProcedure`;

DELIMITER $$

CREATE PROCEDURE `CreatePatientProcedure`(
    IN p_name VARCHAR(100),
    IN p_last_name VARCHAR(50),
    IN p_last_name2 VARCHAR(50),
    IN p_course VARCHAR(100),
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -1; -- Código de error para nombre nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el apellido no sea nulo
    IF p_last_name IS NULL THEN
        SET p_Result = -2; -- Código de error para apellido nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el segundo apellido no sea nulo
    IF p_last_name2 IS NULL THEN
        SET p_Result = -3; -- Código de error para segundo apellido nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el curso no sea nulo
    IF p_course IS NULL THEN
        SET p_Result = -4; -- Código de error para curso nulo
        LEAVE PRO;
    END IF;

    -- Insertar nuevo paciente
    INSERT INTO patients(name, last_name, last_name2, course)
    VALUES(p_name, p_last_name, p_last_name2, p_course);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID del paciente insertado
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Update patient procedure <<===//
DROP PROCEDURE IF EXISTS `UpdatePatientProcedure`;

DELIMITER $$

CREATE PROCEDURE `UpdatePatientProcedure`(
    IN p_id INT,
    IN p_name VARCHAR(100),
    IN p_last_name VARCHAR(50),
    IN p_last_name2 VARCHAR(50),
    IN p_course VARCHAR(100),
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si el paciente existe
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para paciente no encontrado
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

    -- Verificar que el segundo apellido no sea nulo
    IF p_last_name2 IS NULL THEN
        SET p_Result = -4; -- Código de error para segundo apellido nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el curso no sea nulo
    IF p_course IS NULL THEN
        SET p_Result = -5; -- Código de error para curso nulo
        LEAVE PRO;
    END IF;

    -- Actualizar paciente
    UPDATE patients
    SET name = p_name, last_name = p_last_name, last_name2 = p_last_name2, course = p_course
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Delete patient procedure <<===//
DROP PROCEDURE IF EXISTS `DeletePatientProcedure`;

DELIMITER $$
CREATE PROCEDURE `DeletePatientProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si el paciente existe
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar paciente
    DELETE FROM patients WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM patients WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;


-- =====================================
-- ========>>    VISSIT    <<=========
-- =====================================




-- =====================================
-- ========>>    CALENDAR    <<=========
-- =====================================











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





-- *************************************************************************
-- *                                                                       *
-- *                               INSERTS                               *
-- *                                                                       *
-- *************************************************************************

-- ==================================
-- ========>>    USERS    <<=========
-- ==================================
INSERT INTO users (name, password, last_name, email) 
VALUES ('admin', 'AC9689E2272427085E35B9D3E3E8BED88CB3434828B43B86FC0596CAD4C6E270', 'AdminLastName', 'admin@joyfe.com');