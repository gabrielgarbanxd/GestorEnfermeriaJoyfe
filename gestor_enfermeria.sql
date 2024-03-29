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
DROP TABLE IF EXISTS `gestor_enfermeria`.`scheduled_cites_rules`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`visits_templates`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`cites`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`visits`;
DROP TABLE IF EXISTS `gestor_enfermeria`.`patients`;

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
    INDEX `fk_cites_patient_idx` (`patient_id` ASC) VISIBLE,
    INDEX `fk_cites_visit_idx` (`visit_id` ASC) VISIBLE,
    CONSTRAINT `fk_cites_patient`
        FOREIGN KEY (`patient_id`)
        REFERENCES `gestor_enfermeria`.`patients` (`id`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_cites_visit`
        FOREIGN KEY (`visit_id`)
        REFERENCES `gestor_enfermeria`.`visits` (`id`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE = InnoDB;



-- Scheduled Cites Rules
CREATE TABLE `gestor_enfermeria`.`scheduled_cites_rules` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `patient_id` INT UNSIGNED NOT NULL,
    `start_date` DATETIME,
    `end_date` DATETIME,
    `hour` TIME NOT NULL,
    `lunes` INT UNSIGNED NOT NULL,
    `martes` INT UNSIGNED NOT NULL,
    `miercoles` INT UNSIGNED NOT NULL,
    `jueves` INT UNSIGNED NOT NULL,
    `viernes` INT UNSIGNED NOT NULL,
    `visit_template_id` INT UNSIGNED,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
    INDEX `fk_patient_idx_2` (`patient_id` ASC) VISIBLE,
    CONSTRAINT `fk_patient_2`
        FOREIGN KEY (`patient_id`)
        REFERENCES `gestor_enfermeria`.`patients` (`id`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE = InnoDB;


-- Visits Templates
CREATE TABLE `gestor_enfermeria`.`visits_templates` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `type` ENUM('Agudo', 'Crónico') NOT NULL,
    `classification` VARCHAR(255) NOT NULL,
    `description` TEXT NOT NULL,
    `is_comunicated` INT UNSIGNED NOT NULL,
    `is_derived` INT UNSIGNED NOT NULL,
    `trauma_type` ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES') NULL,
    `place` ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS') NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE
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
    SELECT * FROM visits WHERE patient_id = p_patient_id ORDER BY date ASC;

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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE patient_id = p_patient_id  ORDER BY date ASC;

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
    SELECT * FROM visits WHERE DATE(date) = p_date  ORDER BY date ASC;

END$$

DELIMITER ;

-- //===>> GetVisitsByDatePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDatePaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsByDatePaginatedProcedure`(
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE DATE(date) = p_date;

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
    SELECT * FROM visits WHERE date BETWEEN p_start_date AND p_end_date  ORDER BY date ASC;

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
    SELECT * FROM visits WHERE patient_id = p_patient_id AND date BETWEEN p_start_date AND p_end_date  ORDER BY date ASC;

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
    IN p_date DATE
)

PRO : BEGIN

    -- Obtener las visitas por ID de paciente y fecha
    SELECT * FROM visits WHERE patient_id = p_patient_id AND DATE(date) = p_date  ORDER BY date ASC;

END$$

DELIMITER ;

-- //===>> GetVisitsByPatientIdAndDatePaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByPatientIdAndDatePaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByPatientIdAndDatePaginatedProcedure`(
    IN p_patient_id INT,
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE patient_id = p_patient_id AND DATE(date) = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits WHERE patient_id = p_patient_id AND DATE(date) = p_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;


-- //===>> GetVisitsByDayWithPatientInfoProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDayWithPatientInfoProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByDayWithPatientInfoProcedure`(
    IN p_date DATE
)

PRO : BEGIN

    -- Obtener las visitas por fecha con información del paciente
    SELECT v.id, v.type, v.classification, v.description, v.is_comunicated, v.is_derived, v.trauma_type, v.place, v.date, p.id AS patient_id, GetPatientInfoFunction(p.id) AS patient_info
    FROM visits v
    INNER JOIN patients p ON v.patient_id = p.id
    WHERE DATE(v.date) = p_date
    ORDER BY v.date ASC;

END$$

DELIMITER ;

-- //===>> GetVisitsByDayWithPatientInfoPaginatedProcedure visits procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsByDayWithPatientInfoPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetVisitsByDayWithPatientInfoPaginatedProcedure`(
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records FROM visits WHERE DATE(date) = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT v.id, v.type, v.classification, v.description, v.is_comunicated, v.is_derived, v.trauma_type, v.place, v.date, p.id AS patient_id, GetPatientInfoFunction(p.id) AS patient_info
    FROM visits v
    INNER JOIN patients p ON v.patient_id = p.id
    WHERE DATE(v.date) = p_date
    ORDER BY v.date ASC
    LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;


-- =====================================
-- ========>> SCHEDULED CITES RULES <<===
-- =====================================

-- //===>> GetAllScheduledCiteRulesProcedure scheduled_cites_rules procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllScheduledCiteRulesProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllScheduledCiteRulesProcedure`()
PRO : BEGIN

    -- Obtener todas las reglas de citas programadas
    SELECT * FROM scheduled_cites_rules;

END$$

DELIMITER ;

-- //===>> GetAllScheduledCiteRulesPaginatedProcedure scheduled_cites_rules procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllScheduledCiteRulesPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllScheduledCiteRulesPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM scheduled_cites_rules;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM scheduled_cites_rules LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetScheduledCiteRuleByIdProcedure scheduled_cites_rules procedure <<===//
DROP PROCEDURE IF EXISTS `GetScheduledCiteRuleByIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetScheduledCiteRuleByIdProcedure`(
    IN p_id INT
)
PRO : BEGIN

    -- Obtener la regla de citas programadas por ID
    SELECT * FROM scheduled_cites_rules WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> Create scheduled_cites_rule procedure <<===//
DROP PROCEDURE IF EXISTS `CreateScheduledCiteRuleProcedure`;

DELIMITER $$
CREATE PROCEDURE `CreateScheduledCiteRuleProcedure`(
    IN p_name VARCHAR(100),
    IN p_hour TIME,
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_lunes INT,
    IN p_martes INT,
    IN p_miercoles INT,
    IN p_jueves INT,
    IN p_viernes INT,
    IN p_patient_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -1; -- Código de error para nombre nulo
        LEAVE PRO;
    END IF;

    -- Verificar que la hora no sea nula
    IF p_hour IS NULL THEN
        SET p_Result = -2; -- Código de error para hora nula
        LEAVE PRO;
    END IF;

    -- Verificar que la fecha de inicio no sea nula
    IF p_start_date IS NULL THEN
        SET p_Result = -3; -- Código de error para fecha de inicio nula
        LEAVE PRO;
    END IF;

    -- Verificar que la fecha de fin no sea nula
    IF p_end_date IS NULL THEN
        SET p_Result = -4; -- Código de error para fecha de fin nula
        LEAVE PRO;
    END IF;

    -- Verificar que al menos un día de la semana esté seleccionado
    IF p_lunes = 0 AND p_martes = 0 AND p_miercoles = 0 AND p_jueves = 0 AND p_viernes = 0 THEN
        SET p_Result = -5; -- Código de error para días de la semana no seleccionados
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -6; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Insertar nueva regla de citas programadas
    INSERT INTO scheduled_cites_rules(name, hour, start_date, end_date, patient_id)
    VALUES(p_name, p_hour, p_start_date, p_end_date, p_patient_id);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID de la regla de citas programadas insertada
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> Update scheduled_cites_rule procedure <<===//
DROP PROCEDURE IF EXISTS `UpdateScheduledCiteRuleProcedure`;

DELIMITER $$

CREATE PROCEDURE `UpdateScheduledCiteRuleProcedure`(
    IN p_id INT,
    IN p_name VARCHAR(100),
    IN p_hour TIME,
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_lunes INT,
    IN p_martes INT,
    IN p_miercoles INT,
    IN p_jueves INT,
    IN p_viernes INT,
    IN p_patient_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la regla de citas programadas existe
    IF NOT EXISTS (SELECT * FROM scheduled_cites_rules WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para regla de citas programadas no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -2; -- Código de error para nombre nulo
        LEAVE PRO;
    END IF;

    -- Verificar que la hora no sea nula
    IF p_hour IS NULL THEN
        SET p_Result = -3; -- Código de error para hora nula
        LEAVE PRO;
    END IF;

    -- Verificar que la fecha de inicio no sea nula
    IF p_start_date IS NULL THEN
        SET p_Result = -4; -- Código de error para fecha de inicio nula
        LEAVE PRO;
    END IF;

    -- Verificar que la fecha de fin no sea nula
    IF p_end_date IS NULL THEN
        SET p_Result = -5; -- Código de error para fecha de fin nula
        LEAVE PRO;
    END IF;

    -- Verificar que al menos un día de la semana esté seleccionado
    IF p_lunes = 0 AND p_martes = 0 AND p_miercoles = 0 AND p_jueves = 0 AND p_viernes = 0 THEN
        SET p_Result = -6; -- Código de error para días de la semana no seleccionados
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -7; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Actualizar regla de citas programadas
    UPDATE scheduled_cites_rules
    SET name = p_name, hour = p_hour, start_date = p_start_date, end_date = p_end_date, patient_id = p_patient_id
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;


-- //===>> Delete scheduled_cites_rule procedure <<===//
DROP PROCEDURE IF EXISTS `DeleteScheduledCiteRuleProcedure`;

DELIMITER $$

CREATE PROCEDURE `DeleteScheduledCiteRuleProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la regla de citas programadas existe
    IF NOT EXISTS (SELECT * FROM scheduled_cites_rules WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para regla de citas programadas no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar regla de citas programadas
    DELETE FROM scheduled_cites_rules WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM scheduled_cites_rules WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;

-- //===>> SearchScheduledCiteRuleByPatientIdProcedure scheduled_cites_rules procedure <<===//
DROP PROCEDURE IF EXISTS `SearchScheduledCiteRuleByPatientIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `SearchScheduledCiteRuleByPatientIdProcedure`(
    IN p_patient_id INT
)

PRO : BEGIN

    -- Obtener las reglas de citas programadas por ID de paciente
    SELECT * FROM scheduled_cites_rules WHERE patient_id = p_patient_id ORDER BY hour ASC;

END$$

DELIMITER ;

-- //===>> SearchScheduledCiteRuleByPatientIdPaginatedProcedure scheduled_cites_rules procedure <<===//
DROP PROCEDURE IF EXISTS `SearchScheduledCiteRuleByPatientIdPaginatedProcedure`;

DELIMITER $$

CREATE PROCEDURE `SearchScheduledCiteRuleByPatientIdPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM scheduled_cites_rules WHERE patient_id = p_patient_id;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM scheduled_cites_rules WHERE patient_id = p_patient_id LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;



-- ==================================
-- ========>>    CITES    <<=========
-- ==================================

-- //===>> GetAllCitesProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllCitesProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllCitesProcedure`()
PRO : BEGIN

    -- Obtener todas las citas
    SELECT * FROM cites ORDER BY date DESC;

END$$

DELIMITER ;

-- //===>> GetAllCitesPaginatedProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllCitesPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllCitesPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM cites;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM cites LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetCiteByIdProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCiteByIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCiteByIdProcedure`(
    IN p_id INT
)
PRO : BEGIN

    -- Obtener la cita por ID
    SELECT * FROM cites WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> CreateCiteProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `CreateCiteProcedure`;

DELIMITER $$
CREATE PROCEDURE `CreateCiteProcedure`(
    IN p_date DATETIME,
    IN p_patient_id INT,
    IN p_note TEXT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar que la fecha no sea nula
    IF p_date IS NULL THEN
        SET p_Result = -1; -- Código de error para fecha nula
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -2; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Insertar nueva cita
    INSERT INTO cites(date, patient_id, note)
    VALUES(p_date, p_patient_id, p_note);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID de la cita insertada
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> UpdateCiteProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `UpdateCiteProcedure`;

DELIMITER $$
CREATE PROCEDURE `UpdateCiteProcedure`(
    IN p_id INT,
    IN p_date DATETIME,
    IN p_patient_id INT,
    IN p_note TEXT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la cita existe
    IF NOT EXISTS (SELECT * FROM cites WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para cita no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que la fecha no sea nula
    IF p_date IS NULL THEN
        SET p_Result = -2; -- Código de error para fecha nula
        LEAVE PRO;
    END IF;

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -3; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Actualizar cita
    UPDATE cites
    SET date = p_date, patient_id = p_patient_id, note = p_note
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> DeleteCiteProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `DeleteCiteProcedure`;

DELIMITER $$
CREATE PROCEDURE `DeleteCiteProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la cita existe
    IF NOT EXISTS (SELECT * FROM cites WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para cita no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar cita
    DELETE FROM cites WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM cites WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;

-- //===>> GetCitesByPatientIdProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByPatientIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByPatientIdProcedure`(
    IN p_patient_id INT
)
PRO : BEGIN

    -- Obtener las citas por ID de paciente
    SELECT * FROM cites WHERE patient_id = p_patient_id ORDER BY date DESC;

END$$

DELIMITER ;

-- //===>> GetCitesByPatientIdPaginatedProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByPatientIdPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByPatientIdPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM cites WHERE patient_id = p_patient_id;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM cites WHERE patient_id = p_patient_id LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetCitesByDateProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByDayProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayProcedure`(
    IN p_date DATE
)
PRO : BEGIN

    -- Obtener las citas por fecha
    SELECT * FROM cites WHERE DATE(date) = p_date ORDER BY date DESC;

END$$

DELIMITER ;

-- //===>> GetCitesByDatePaginatedProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByDayPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayPaginatedProcedure`(
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records FROM cites WHERE DATE(date) = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM cites WHERE DATE(date) = p_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetCitesByDayAndPatientIdProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByDayAndPatientIdProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayAndPatientIdProcedure`(
    IN p_patient_id INT,
    IN p_date DATE
)

PRO : BEGIN

    -- Obtener las citas por ID de paciente y fecha
    SELECT * FROM cites WHERE patient_id = p_patient_id AND DATE(date) = p_date ORDER BY date DESC;

END$$

DELIMITER ;

-- //===>> GetCitesByDayAndPatientIdPaginatedProcedure cites procedure <<===//
DROP PROCEDURE IF EXISTS `GetCitesByDayAndPatientIdPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayAndPatientIdPaginatedProcedure`(
    IN p_patient_id INT,
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records FROM cites WHERE patient_id = p_patient_id AND DATE(date) = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM cites WHERE patient_id = p_patient_id AND DATE(date) = p_date LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;


-- //===>> GetCitesByDayWithPatientInfoProcedure
DROP PROCEDURE IF EXISTS `GetCitesByDayWithPatientInfoProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayWithPatientInfoProcedure`(
    IN p_date DATE
)

PRO : BEGIN

    -- Obtener las citas por fecha con información del paciente
    SELECT cites.id, cites.date, cites.note, cites.visit_id, patients.id AS patient_id, GetPatientInfoFunction(patients.id) AS patient_info
    FROM cites
    INNER JOIN patients ON cites.patient_id = patients.id
    WHERE DATE(cites.date) = p_date
    ORDER BY cites.date DESC;

END$$

DELIMITER ;

-- //===>> GetCitesByDayWithPatientInfoPaginatedProcedure
DROP PROCEDURE IF EXISTS `GetCitesByDayWithPatientInfoPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetCitesByDayWithPatientInfoPaginatedProcedure`(
    IN p_date DATE,
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
    SELECT COUNT(*) INTO @total_records
    FROM cites
    WHERE DATE(cites.date) = p_date;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT cites.id, cites.date, cites.note, cites.visit_id, patients.id AS patient_id, GetPatientInfoFunction(patients.id) AS patient_info
    FROM cites
    INNER JOIN patients ON cites.patient_id = patients.id
    WHERE DATE(cites.date) = p_date
    ORDER BY cites.date DESC
    LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetAllWithPatientInfoProcedure
DROP PROCEDURE IF EXISTS `GetAllCitesWithPatientInfoProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllCitesWithPatientInfoProcedure`()
PRO : BEGIN

    -- Obtener todas las citas con información del paciente
    SELECT cites.id, cites.date, cites.note, cites.visit_id, patients.id AS patient_id, GetPatientInfoFunction(patients.id) AS patient_info
    FROM cites
    INNER JOIN patients ON cites.patient_id = patients.id
    ORDER BY cites.date DESC;

END$$

DELIMITER ;

-- //===>> GetAllWithPatientInfoPaginatedProcedure
DROP PROCEDURE IF EXISTS `GetAllCitesWithPatientInfoPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllCitesWithPatientInfoPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM cites;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT cites.id, cites.date, cites.note, cites.visit_id, patients.id AS patient_id, GetPatientInfoFunction(patients.id) AS patient_info
    FROM cites
    INNER JOIN patients ON cites.patient_id = patients.id
    ORDER BY cites.date DESC
    LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;



-- ==================================
-- =====>> VISITS TEMPLATES    <<====
-- ==================================

-- //===>> GetAllVisitsTemplatesProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllVisitsTemplatesProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetAllVisitsTemplatesProcedure`()
PRO : BEGIN

    -- Obtener todas las plantillas de visitas
    SELECT * FROM visits_templates;

END$$

DELIMITER ;

-- //===>> GetAllVisitsTemplatesPaginatedProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `GetAllVisitsTemplatesPaginatedProcedure`;

DELIMITER $$
CREATE PROCEDURE `GetAllVisitsTemplatesPaginatedProcedure`(
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
    SELECT COUNT(*) INTO @total_records FROM visits_templates;

    -- Verificar que el número de registros a omitir sea menor al número total de registros
    IF p_offset >= @total_records THEN
        SET p_Result = -3; -- Código de error para número de página inválido
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Obtener los registros de la página actual
    SELECT * FROM visits_templates LIMIT p_per_page OFFSET p_offset;

    -- Devolver el número total de registros
    SET p_Result = @total_records;

END$$

DELIMITER ;

-- //===>> GetVisitsTemplateByIdProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `GetVisitsTemplateByIdProcedure`;

DELIMITER $$

CREATE PROCEDURE `GetVisitsTemplateByIdProcedure`(
    IN p_id INT
)

PRO : BEGIN

    -- Obtener la plantilla de visitas por ID
    SELECT * FROM visits_templates WHERE id = p_id;

END$$

DELIMITER ;

-- //===>> CreateVisitsTemplateProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `CreateVisitsTemplateProcedure`;

DELIMITER $$

CREATE PROCEDURE `CreateVisitsTemplateProcedure`(
    IN p_name VARCHAR(100),
    IN p_type ENUM('Agudo', 'Crónico'),
    IN p_classification VARCHAR(255),
    IN p_description TEXT,
    IN p_is_comunicated INT,
    IN p_is_derived INT,
    IN p_trauma_type ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES'),
    IN p_place ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS'),
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -1; -- Código de error para nombre nulo
        LEAVE PRO;
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

    -- Verificar que el campo de comunicado no sea nulo
    IF p_is_comunicated IS NULL THEN
        SET p_Result = -5; -- Código de error para campo de comunicado nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el campo de derivado no sea nulo
    IF p_is_derived IS NULL THEN
        SET p_Result = -6; -- Código de error para campo de derivado nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el tipo de trauma no sea nulo
    IF p_trauma_type IS NULL THEN
        SET p_Result = -7; -- Código de error para tipo de trauma nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el lugar no sea nulo
    IF p_place IS NULL THEN
        SET p_Result = -8; -- Código de error para lugar nulo
        LEAVE PRO;
    END IF;

    -- Insertar nueva plantilla de visitas
    INSERT INTO visits_templates(name, type, classification, description, is_comunicated, is_derived, trauma_type, place)
    VALUES(p_name, p_type, p_classification, p_description, p_is_comunicated, p_is_derived, p_trauma_type, p_place);

    -- Verificar si la inserción fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = LAST_INSERT_ID(); -- Devolver el ID de la plantilla de visitas insertada
    ELSE
        SET p_Result = 0; -- Código de error para inserción no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> UpdateVisitsTemplateProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `UpdateVisitsTemplateProcedure`;

DELIMITER $$

CREATE PROCEDURE `UpdateVisitsTemplateProcedure`(
    IN p_id INT,
    IN p_name VARCHAR(100),
    IN p_type ENUM('Agudo', 'Crónico'),
    IN p_classification VARCHAR(255),
    IN p_description TEXT,
    IN p_is_comunicated INT,
    IN p_is_derived INT,
    IN p_trauma_type ENUM('BUCODENTAL/MAXILOFACIAL', 'CUERPO EXTRAÑO (INGESTA/OTROS)', 'BRECHAS', 'TEC', 'CARA', 'ROTURA DE GAFAS', 'TRAUMATOLOGÍA MIEMBRO INFERIOR', 'TRAUMATOLOGÍA MIEMBRO SUPERIOR', 'OTROS ACCIDENTES'),
    IN p_place ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS'),
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la plantilla de visitas existe
    IF NOT EXISTS (SELECT * FROM visits_templates WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para plantilla de visitas no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el nombre no sea nulo
    IF p_name IS NULL THEN 
        SET p_Result = -2; -- Código de error para nombre nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el tipo no sea nulo
    IF p_type IS NULL THEN
        SET p_Result = -3; -- Código de error para tipo nulo
        LEAVE PRO;
    END IF;

    -- Verificar que la clasificación no sea nula
    IF p_classification IS NULL THEN
        SET p_Result = -4; -- Código de error para clasificación nula
        LEAVE PRO;
    END IF;

    -- Verificar que la descripción no sea nula
    IF p_description IS NULL THEN
        SET p_Result = -5; -- Código de error para descripción nula
        LEAVE PRO;
    END IF;

    -- Verificar que el campo de comunicado no sea nulo
    IF p_is_comunicated IS NULL THEN
        SET p_Result = -6; -- Código de error para campo de comunicado nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el campo de derivado no sea nulo
    IF p_is_derived IS NULL THEN
        SET p_Result = -7; -- Código de error para campo de derivado nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el tipo de trauma no sea nulo
    IF p_trauma_type IS NULL THEN
        SET p_Result = -8; -- Código de error para tipo de trauma nulo
        LEAVE PRO;
    END IF;

    -- Verificar que el lugar no sea nulo
    IF p_place IS NULL THEN
        SET p_Result = -9; -- Código de error para lugar nulo
        LEAVE PRO;
    END IF;

    -- Actualizar plantilla de visitas
    UPDATE visits_templates
    SET name = p_name, type = p_type, classification = p_classification, description = p_description, is_comunicated = p_is_comunicated, is_derived = p_is_derived, trauma_type = p_trauma_type, place = p_place
    WHERE id = p_id;

    -- Verificar si la actualización fue exitosa
    IF ROW_COUNT() > 0 THEN
        SET p_Result = 1; -- Código de éxito
    ELSE
        SET p_Result = 0; -- Código de error para actualización no exitosa
    END IF;

END$$

DELIMITER ;

-- //===>> DeleteVisitsTemplateProcedure visits_templates procedure <<===//
DROP PROCEDURE IF EXISTS `DeleteVisitsTemplateProcedure`;

DELIMITER $$
CREATE PROCEDURE `DeleteVisitsTemplateProcedure`(
    IN p_id INT,
    OUT p_Result INT
)

PRO : BEGIN

    -- Verificar si la plantilla de visitas existe
    IF NOT EXISTS (SELECT * FROM visits_templates WHERE id = p_id) THEN
        SET p_Result = -1; -- Código de error para plantilla de visitas no encontrada
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Eliminar plantilla de visitas
    DELETE FROM visits_templates WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    SELECT id INTO p_Result FROM visits_templates WHERE id = p_id;

    -- Verificar si la eliminación fue exitosa
    IF p_Result > 0 THEN
        SET p_Result = 0; -- Código de error para eliminación no exitosa
    ELSE
        SET p_Result = p_id; -- Código de éxito
    END IF;

END$$

DELIMITER ;





-- ==================================
-- ========>>    OTHERS    <<========
-- ==================================

-- //===>> CreateAllCitesForNewScheduledCiteRuleProcedure procedure <<===//
DROP PROCEDURE IF EXISTS `CreateAllCitesForNewScheduledCiteRuleProcedure`;

DELIMITER $$
CREATE PROCEDURE `CreateAllCitesForNewScheduledCiteRuleProcedure`(
    IN p_patient_id INT,
    IN p_hour TIME,
    IN p_days ENUM('Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes'),
    IN p_start_date DATE,
    IN p_end_date DATE,
    OUT p_Result INT
)

PRO : BEGIN

    DECLARE p_current_date DATE;
    DECLARE p_current_day ENUM('Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes');

    -- Verificar que el paciente exista
    IF NOT EXISTS (SELECT * FROM patients WHERE id = p_patient_id) THEN
        SET p_Result = -1; -- Código de error para paciente no encontrado
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que la hora no sea nula
    IF p_hour IS NULL THEN
        SET p_Result = -2; -- Código de error para hora nula
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el día de inicio no sea nulo
    IF p_start_date IS NULL THEN
        SET p_Result = -3; -- Código de error para día de inicio nulo
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el día de fin no sea nulo
    IF p_end_date IS NULL THEN
        SET p_Result = -4; -- Código de error para día de fin nulo
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Verificar que el día de inicio sea menor o igual al día de fin
    IF p_start_date > p_end_date THEN
        SET p_Result = -5; -- Código de error para día de inicio mayor al día de fin
        LEAVE PRO; -- Salir del procedimiento almacenado
    END IF;

    -- Inicializar la fecha actual
    SET p_current_date = p_start_date;

    -- Mientras la fecha actual sea menor o igual a la fecha de fin
    WHILE p_current_date <= p_end_date DO

        -- Obtener el día de la semana de la fecha actual
        SET p_current_day = DAYNAME(p_current_date);

        -- Verificar si el día de la semana de la fecha actual es igual al día de la regla
        IF p_current_day = p_days THEN

            -- Insertar nueva cita
            INSERT INTO cites(date, patient_id, note)
            VALUES(CONCAT(p_current_date, ' ', p_hour), p_patient_id, NULL);

        END IF;

        -- Añadir un día a la fecha actual
        SET p_current_date = DATE_ADD(p_current_date, INTERVAL 1 DAY);

    END WHILE;

    -- Devolver el número total de citas creadas
    SET p_Result = ROW_COUNT();

END$$

DELIMITER ;


-- *************************************************************************
-- *                                                                       *
-- *                                 VIEWS                               *
-- *                                                                       *
-- *************************************************************************




-- =====>>Patient Active Cites <<====
DROP VIEW IF EXISTS patient_active_cites;

CREATE VIEW patient_active_cites AS
SELECT c.id AS cite_id, p.name AS patient_name, c.date, vt.name AS template_name
FROM `gestor_enfermeria`.`cites` c
INNER JOIN `gestor_enfermeria`.`patients` p ON c.patient_id = p.id
INNER JOIN `gestor_enfermeria`.`visits_templates` vt ON c.visit_template_id = vt.id
WHERE c.date >= CURDATE();

-- select * from patient_active_cites;


-- =====>>Scheduled Cites by Weekday <<====
DROP VIEW IF EXISTS scheduled_cites_by_weekday;

CREATE VIEW scheduled_cites_by_weekday AS
SELECT p.name AS patient_name, scr.start_date, scr.hour, 
CASE WHEN scr.lunes = 1 THEN 'Lunes' ELSE '' END AS lunes,
CASE WHEN scr.martes = 1 THEN 'Martes' ELSE '' END AS martes,
CASE WHEN scr.miercoles = 1 THEN 'Miércoles' ELSE '' END AS miercoles,
CASE WHEN scr.jueves = 1 THEN 'Jueves' ELSE '' END AS jueves,
CASE WHEN scr.viernes = 1 THEN 'Viernes' ELSE '' END AS viernes
FROM `gestor_enfermeria`.`scheduled_cites_rules` scr
INNER JOIN `gestor_enfermeria`.`patients` p ON scr.patient_id = p.id;


-- select * from scheduled_cites_by_weekday;





-- =====>>Vista de Visitas por Tipo <<====
DROP VIEW IF EXISTS vw_visits_by_type;

CREATE VIEW vw_visits_by_type AS
SELECT type AS tipo_visita, COUNT(*) AS cantidad
FROM visits
GROUP BY type;

-- select * from scheduled_cites_by_weekday;


-- =====>>Vista de Citas con detalles del paciente <<====
DROP VIEW IF EXISTS vw_cites_with_patient_details;


CREATE VIEW vw_cites_with_patient_details AS
SELECT c.id AS cita_id, c.date AS fecha_cita, p.name AS nombre_paciente, p.last_name AS apellido_paciente, c.note AS nota
FROM cites c
JOIN patients p ON c.patient_id = p.id;

-- select * from vw_cites_with_patient_details;



-- =====>>Vista de Citas Pendientes<<====
DROP VIEW IF EXISTS vw_pending_cites;

CREATE VIEW vw_pending_cites AS
SELECT c.id AS cita_id, p.name AS nombre_paciente, p.last_name AS apellido_paciente, c.date AS fecha_cita
FROM cites c
JOIN patients p ON c.patient_id = p.id
WHERE c.date > NOW();

-- select * from vw_acute_visits;





-- =====>>Vista de Visitas Agudas<<====
DROP VIEW IF EXISTS vw_acute_visits;

CREATE VIEW vw_acute_visits AS
SELECT id, date, classification, description, patient_id
FROM visits
WHERE type = 'Agudo';

-- select * from vw_acute_visits;


-- =====>>Vista de Pacientes con Citas Pendientes<<====
DROP VIEW IF EXISTS vw_patients_with_pending_cites;


CREATE VIEW vw_patients_with_pending_cites AS
SELECT p.id AS paciente_id, p.name AS nombre_paciente, p.last_name AS apellido_paciente, COUNT(c.id) AS citas_pendientes
FROM patients p
LEFT JOIN cites c ON p.id = c.patient_id AND c.date > NOW()
GROUP BY p.id;

-- select * from vw_patients_with_pending_cites;

-- =====>>Daily Cites Grouped by Place and Trauma Type<<====
DROP VIEW IF EXISTS daily_cites_grouped_by_place_trauma_type;

CREATE VIEW daily_cites_grouped_by_place_trauma_type AS
SELECT place, trauma_type, DATE(date) AS visit_date, COUNT(*) AS daily_cite_count
FROM `gestor_enfermeria`.`visits` v
WHERE trauma_type IS NOT NULL
GROUP BY place, trauma_type, DATE(date)
ORDER BY visit_date ASC;


-- select * from daily_cites_grouped_by_place_trauma_type;



-- =====>> Most Frequent Trauma Types<<====
DROP VIEW IF EXISTS most_frequent_trauma_types;


CREATE VIEW most_frequent_trauma_types AS
SELECT trauma_type, COUNT(*) AS trauma_count
FROM `gestor_enfermeria`.`visits` v
WHERE trauma_type IS NOT NULL
GROUP BY trauma_type
ORDER BY trauma_count DESC
LIMIT 5;

-- select * from most_frequent_trauma_types;


-- =====>>Vista de Citas Canceladas<<====
DROP VIEW IF EXISTS vw_cancelled_cites;


CREATE VIEW vw_cancelled_cites AS
SELECT c.id AS cita_id, c.date AS fecha_cita, p.name AS nombre_paciente, p.last_name AS apellido_paciente, c.note AS motivo_cancelacion
FROM cites c
JOIN patients p ON c.patient_id = p.id
WHERE c.date < NOW() AND c.note IS NOT NULL;

-- select * from vw_cancelled_cites;



-- =====>>Vista de Visitas Crónicas Realizadas<<====
DROP VIEW IF EXISTS vw_completed_chronic_visits;

CREATE VIEW vw_completed_chronic_visits AS
SELECT id, date, classification, description, patient_id
FROM visits
WHERE type = 'Crónico' AND date < NOW();

-- select * from vw_completed_chronic_visits;



-- =====>>Vista de Citas por Mes<<====
DROP VIEW IF EXISTS vw_cites_by_month;

CREATE VIEW vw_cites_by_month AS
SELECT MONTH(date) AS mes, COUNT(*) AS cantidad_citas
FROM cites
GROUP BY mes;

-- select * from vw_cites_by_month;



-- =====>>Cites by Weekday and Hour<<====
DROP VIEW IF EXISTS cites_by_weekday_hour;


CREATE VIEW cites_by_weekday_hour AS
SELECT DATE_FORMAT(date, '%W') AS weekday, HOUR(date) AS hour, COUNT(*) AS cite_count
FROM `gestor_enfermeria`.`cites`
GROUP BY DATE_FORMAT(date, '%W'), HOUR(date)
ORDER BY weekday, hour;



-- *************************************************************************
-- *                                                                       *
-- *                               FUNCTIONS                             *
-- *                                                                       *
-- *************************************************************************

-- //===>> GetPatientInfoFunction function <<===//
DROP FUNCTION IF EXISTS `GetPatientInfoFunction`;

DELIMITER $$
CREATE FUNCTION `GetPatientInfoFunction`(
    p_patient_id INT
)
RETURNS TEXT
DETERMINISTIC
BEGIN
    DECLARE p_info TEXT;

    -- Obtener la información del paciente
    SELECT CONCAT(name, ' ', last_name, ' ', course) INTO p_info FROM patients WHERE id = p_patient_id;

    -- Devolver la información del paciente
    RETURN p_info;
END$$

DELIMITER ;


-- //===>> GetTotalVisitsFunction function <<===//
DROP FUNCTION IF EXISTS `GetTotalVisitsFunction`;
DELIMITER $$
CREATE FUNCTION `GetTotalVisitsFunction`(
    p_patient_id INT
)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE total_visits INT;

    -- Contar el número total de visitas del paciente
    SELECT COUNT(*) INTO total_visits FROM visits WHERE patient_id = p_patient_id;

    -- Devolver el número total de visitas
    RETURN total_visits;
END$$
DELIMITER ;



-- //===>> GetNextAppointmentFunction function <<===//
DROP FUNCTION IF EXISTS `GetNextAppointmentFunction`;
DELIMITER $$
CREATE FUNCTION `GetNextAppointmentFunction`(
    p_patient_id INT
)
RETURNS DATETIME
DETERMINISTIC
BEGIN
    DECLARE next_appointment DATETIME;

    -- Obtener el próximo horario de cita del paciente
    SELECT MIN(date) INTO next_appointment FROM cites WHERE patient_id = p_patient_id AND date > NOW();

    -- Devolver el próximo horario de cita
    RETURN next_appointment;
END$$
DELIMITER ;



-- //===>> GetAverageVisitDurationFunction function <<===//
DROP FUNCTION IF EXISTS `GetAverageVisitDurationFunction`;
DELIMITER $$
CREATE FUNCTION `GetAverageVisitDurationFunction`(
    p_patient_id INT
)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    DECLARE avg_duration DECIMAL(10,2);

    -- Calcular la duración promedio de las visitas del paciente
    SELECT AVG(TIMESTAMPDIFF(MINUTE, date, end_date)) INTO avg_duration FROM visits WHERE patient_id = p_patient_id;

    -- Devolver la duración promedio de la visita
    RETURN avg_duration;
END$$
DELIMITER ;



-- //===>> GetTotalVisitsByTypeFunction function <<===//
DROP FUNCTION IF EXISTS `GetTotalVisitsByTypeFunction`;
DELIMITER $$
CREATE FUNCTION `GetTotalVisitsByTypeFunction`(
    p_patient_id INT,
    p_visit_type ENUM('Agudo', 'Crónico')
)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE total_visits INT;

    -- Contar el número total de visitas del paciente por tipo
    SELECT COUNT(*) INTO total_visits FROM visits WHERE patient_id = p_patient_id AND type = p_visit_type;

    -- Devolver el número total de visitas por tipo
    RETURN total_visits;
END$$
DELIMITER ;



-- //===>> GetTotalVisitsByTraumaTypeFunction function <<===//
DROP FUNCTION IF EXISTS `GetAverageTimeBetweenAppointmentsFunction`;
DELIMITER $$
CREATE FUNCTION `GetAverageTimeBetweenAppointmentsFunction`(
    p_patient_id INT
)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    DECLARE avg_time DECIMAL(10,2);

    -- Calcular el tiempo promedio entre citas del paciente
    SELECT AVG(TIMESTAMPDIFF(DAY, date, (SELECT MIN(date) FROM cites WHERE patient_id = p_patient_id AND date > t.date))) INTO avg_time 
    FROM cites t WHERE patient_id = p_patient_id;

    -- Devolver el tiempo promedio entre citas
    RETURN avg_time;
END$$
DELIMITER ;



-- //===>> GetTotalVisitsByTraumaTypeFunction function <<===//
DROP FUNCTION IF EXISTS `GetLastVisitDateFunction`;
DELIMITER $$
CREATE FUNCTION `GetLastVisitDateFunction`(
    p_patient_id INT
)
RETURNS DATETIME
DETERMINISTIC
BEGIN
    DECLARE last_visit_date DATETIME;

    -- Obtener la fecha de la última visita del paciente
    SELECT MAX(date) INTO last_visit_date FROM visits WHERE patient_id = p_patient_id;

    -- Devolver la fecha de la última visita
    RETURN last_visit_date;
END$$
DELIMITER ;



-- //===>> GetTotalVisitsByTraumaTypeFunction function <<===//
DROP FUNCTION IF EXISTS `GetTotalVisitsByPlaceFunction`;
DELIMITER $$
CREATE FUNCTION `GetTotalVisitsByPlaceFunction`(
    p_patient_id INT,
    p_place ENUM('RECREO', 'ED. FÍSICA', 'CLASE', 'NATACIÓN', 'GUARDERÍA', 'SEMANA DEPORTIVA', 'DÍA VERDE', 'EXTRAESCOLAR', 'OTROS')
)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE total_visits_by_place INT;

    -- Contar el número total de visitas del paciente por lugar
    SELECT COUNT(*) INTO total_visits_by_place FROM visits WHERE patient_id = p_patient_id AND place = p_place;

    -- Devolver el número total de visitas por lugar
    RETURN total_visits_by_place;
END$$
DELIMITER ;







-- ==================================
-- ========>>    USERS    <<=========
-- ==================================
INSERT INTO users (name, password, last_name, email) 
VALUES ('admin', 'AC9689E2272427085E35B9D3E3E8BED88CB3434828B43B86FC0596CAD4C6E270', 'AdminLastName', 'admin@joyfe.com');