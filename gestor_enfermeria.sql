-- drop database gestor_enfermeria;
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
    `is_comunicated` INT UNSIGNED NOT NULL,
    `is_derived` INT UNSIGNED NOT NULL,
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


-- Cites table
CREATE TABLE `gestor_enfermeria`.`cites` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `patient_id` INT UNSIGNED NOT NULL,
    `note` TEXT,
    `visit_id` INT UNSIGNED,
    `date` DATETIME,
    `visit_template_id` INT UNSIGNED,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
    INDEX `fk_paciente_idx` (`patient_id` ASC) VISIBLE,
    INDEX `fk_visita_idx` (`visit_id` ASC) VISIBLE,
    CONSTRAINT `fk_paciente`
        FOREIGN KEY (`patient_id`)
        REFERENCES `gestor_enfermeria`.`patients` (`id`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_visita`
        FOREIGN KEY (`visit_id`)
        REFERENCES `gestor_enfermeria`.`visitas` (`id`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE = InnoDB;







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

-- //===>> GetAllVisitsProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllVisitsProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllVisitsProcedure`()
PRO : BEGIN

    -- Obtener todas las visitas
    SELECT * FROM visits;

END$$

DELIMITER ;

-- //===>> GetAllVisitsPaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllVisitsPaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllVisitsPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM visits;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitByIdProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitByIdProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitByIdProcedure`(
    IN p_id INT
)
PRO : BEGIN

    -- Obtener la visita por ID
    SELECT * FROM visits WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> Create visit procedure <<===//
DROP PROCEDURE IF EXISTS `CreateVisitProcedure`;

DELIMITER $$

CREATE PROCEDURE `CreateVisitProcedure`(
    IN p_type ENUM('Agudo', 'Crónico'),
    IN p_classification VARCHAR(255),
    IN p_description TEXT,
    IN p_is_comunicated INT,
    IN p_is_derived INT,
    IN p_trauma_type ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES'),
    IN p_place ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS'),
    IN p_date DATETIME,
    IN p_patient_id INT,
    OUT p_Result INT
)
PRO : BEGIN

    -- Verificar que el tipo no sea nulo
    IF p_type IS NULL THEN 
        SET p_Result = -1; -- Código de error para tipo nulo
        LEAVE PRO;
    END IF;

    -- Verificar que la clasificación no sea nula
    IF p_classification IS NULL THEN
        SET p_Result = -2; -- Código de error para clasificación nula
        LEAVE PRO;
    END IF;

    -- Verificar que la descripción no sea nula
    IF p_description IS NULL THEN
        SET p_Result = -3; -- Código de error para descripción nula
        LEAVE PRO;
    END IF;

    -- Verificar que el campo is_comunicated no sea nulo
    IF p_is_comunicated IS NULL THEN
        SET p_Result = -4; -- Código de error para is_comunicated nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el campo is_derived no sea nulo
    IF p_is_derived IS NULL THEN
        SET p_Result = -5; -- Código de error para is_derived nulo
        LEAVE PRO;
    END IF;

    -- Verificar si p_is_derived es falso que el campo trauma_type y place no sean nulos
    IF p_is_derived = 0 THEN
        IF p_trauma_type IS NULL THEN
            SET p_Result = -6; -- Código de error para trauma_type nulo
            LEAVE PRO;
        END IF;

        IF p_place IS NULL THEN
            SET p_Result = -7; -- Código de error para place nulo
            LEAVE PRO;
        END IF;
    END IF;

    -- Verificar que el campo date no sea nulo
    IF p_date IS NULL THEN
        SET p_Result = -8; -- Código de error para date nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -9; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Insertar nueva visita
    INSERT INTO visits(type, classification, description, is_comunicated, is_derived, trauma_type, place, date, patient_id)
    VALUES(p_type, p_classification, p_description, p_is_comunicated, p_is_derived, p_trauma_type, p_place, p_date, p_patient_id);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID de la visita insertada
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Update visit procedure <<===//
DROP PROCEDURE IF EXISTS `UpdateVisitProcedure`;

DELIMITER $$
CREATE PROCEDURE `UpdateVisitProcedure`(
    IN p_id INT,
    IN p_type ENUM('Agudo', 'Crónico'),
    IN p_classification VARCHAR(255),
    IN p_description TEXT,
    IN p_is_comunicated INT,
    IN p_is_derived INT,
    IN p_trauma_type ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES'),
    IN p_place ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS'),
    IN p_date DATETIME,
    IN p_patient_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la visita existe
    IF NOT EXISTS (SELECT * FROM visits WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para visita no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el tipo no sea nulo
    IF p_type IS NULL THEN 
        SET p_Result = -2; -- Código de error para tipo nulo
        LEAVE PRO;
    END IF;

    -- Verificar que la clasificación no sea nula
    IF p_classification IS NULL THEN
        SET p_Result = -3; -- Código de error para clasificación nula
        LEAVE PRO;
    END IF;

    -- Verificar que la descripción no sea nula
    IF p_description IS NULL THEN
        SET p_Result = -4; -- Código de error para descripción nula
        LEAVE PRO;
    END IF;

    -- Verificar que el campo is_comunicated no sea nulo
    IF p_is_comunicated IS NULL THEN
        SET p_Result = -5; -- Código de error para is_comunicated nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el campo is_derived no sea nulo
    IF p_is_derived IS NULL THEN
        SET p_Result = -6; -- Código de error para is_derived nulo
        LEAVE PRO;
    END IF;

    -- Verificar si p_is_derived es falso que el campo trauma_type y place no sean nulos
    IF p_is_derived = 0 THEN
        IF p_trauma_type IS NULL THEN
            SET p_Result = -7; -- Código de error para trauma_type nulo
            LEAVE PRO;
        END IF;

        IF p_place IS NULL THEN
            SET p_Result = -8; -- Código de error para place nulo
            LEAVE PRO;
        END IF;
    END IF;

    -- Verificar que el campo date no sea nulo
    IF p_date IS NULL THEN
        SET p_Result = -9; -- Código de error para date nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -10; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Actualizar visita
    UPDATE visits
    SET type = p_type, classification = p_classification, description = p_description, is_comunicated = p_is_comunicated, is_derived = p_is_derived, trauma_type = p_trauma_type, place = p_place, date = p_date, patient_id = p_patient_id
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Delete visit procedure <<===//
DROP PROCEDURE IF EXISTS `DeleteVisitProcedure`;

DELIMITER $$
CREATE PROCEDURE `DeleteVisitProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la visita existe
    IF NOT EXISTS (SELECT * FROM visits WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para visita no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar visita
    DELETE FROM visits WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM visits WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByPatientIdProcedure`(
    IN p_patient_id INT
)
PRO : BEGIN

    -- Obtener las visitas por ID de paciente
    SELECT * FROM visits WHERE patient_id = p_patient_id;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdPaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByPatientIdPaginatedProcedure`(
    IN p_patient_id INT,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE patient_id = p_patient_id;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE patient_id = p_patient_id LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitsByDateProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDateProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByDateProcedure`(
    IN p_date DATE
)
PRO : BEGIN

    -- Obtener las visitas por fecha
    SELECT * FROM visits WHERE DATE(date) = p_date;

END$$

DELIMITER ;

-- //===>> GetVisitsByDatePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDatePaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByDatePaginatedProcedure`(
    IN p_date DATETIME,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE date = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE DATE(date) = p_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitsByDateRangeProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDateRangeProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByDateRangeProcedure`(
    IN p_start_date DATETIME,
    IN p_end_date DATETIME
)

PRO : BEGIN

    -- Obtener las visitas por rango de fechas
    SELECT * FROM visits WHERE date BETWEEN p_start_date AND p_end_date;

END$$

DELIMITER ;

-- //===>> GetVisitsByDateRangePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDateRangePaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByDateRangePaginatedProcedure`(
    IN p_start_date DATETIME,
    IN p_end_date DATETIME,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE date BETWEEN p_start_date AND p_end_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE date BETWEEN p_start_date AND p_end_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdAndDateRangeProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdAndDateRangeProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByPatientIdAndDateRangeProcedure`(
    IN p_patient_id INT,
    IN p_start_date DATETIME,
    IN p_end_date DATETIME
)

PRO : BEGIN

    -- Obtener las visitas por ID de paciente y rango de fechas
    SELECT * FROM visits WHERE patient_id = p_patient_id AND date BETWEEN p_start_date AND p_end_date;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdAndDateRangePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdAndDateRangePaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByPatientIdAndDateRangePaginatedProcedure`(
    IN p_patient_id INT,
    IN p_start_date DATETIME,
    IN p_end_date DATETIME,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE patient_id = p_patient_id AND date BETWEEN p_start_date AND p_end_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE patient_id = p_patient_id AND date BETWEEN p_start_date AND p_end_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdAndDateProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdAndDateProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByPatientIdAndDateProcedure`(
    IN p_patient_id INT,
    IN p_date DATETIME
)

PRO : BEGIN

    -- Obtener las visitas por ID de paciente y fecha
    SELECT * FROM visits WHERE patient_id = p_patient_id AND date = p_date;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdAndDatePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdAndDatePaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByPatientIdAndDatePaginatedProcedure`(
    IN p_patient_id INT,
    IN p_date DATETIME,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE patient_id = p_patient_id AND date = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE patient_id = p_patient_id AND date = p_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$




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