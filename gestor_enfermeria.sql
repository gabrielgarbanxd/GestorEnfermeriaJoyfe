create database if not exists gestor_enfermeria;
use gestor_enfermeria;

-- *************************************************************************
-- *                                                                       *
-- *                             TABLE DEFINITIONS                        *
-- *                                                                       *
-- *************************************************************************

-- Users table
DROP TABLE IF EXISTS `gestor_enfermeria`.`users`;

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
DROP TABLE IF EXISTS `gestor_enfermeria`.`patients`;

CREATE TABLE `gestor_enfermeria`.`patients` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `last_name` VARCHAR(50) NOT NULL,
    `last_name2` VARCHAR(50) NOT NULL,
    `course` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE);



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

-- ==================================
-- ========>>  PATIENTS   <<=========
-- ==================================
insert into patients (name, last_name, last_name2, course) values ('Rosa', 'Toppas', 'Meriguet', '7ºDXQ I');
insert into patients (name, last_name, last_name2, course) values ('Alessandra', 'Gorgl', 'Mannakee', '0ºBPQ R');
insert into patients (name, last_name, last_name2, course) values ('Kayley', 'Comusso', 'Milliere', '1ºRVU W');
insert into patients (name, last_name, last_name2, course) values ('Almeria', 'McBlain', 'Smullin', '7ºACM Z');
insert into patients (name, last_name, last_name2, course) values ('Avery', 'Vondra', 'Dobrovolski', '6ºELX R');
insert into patients (name, last_name, last_name2, course) values ('Gerrie', 'O'' Mara', 'Pople', '7ºLFD H');
insert into patients (name, last_name, last_name2, course) values ('Sheffield', 'Callister', 'Grimmert', '9ºOMD C');
insert into patients (name, last_name, last_name2, course) values ('Willie', 'Snowden', 'Beadle', '2ºIKN D');
insert into patients (name, last_name, last_name2, course) values ('Mamie', 'Storrock', 'Entwistle', '2ºIGS U');
insert into patients (name, last_name, last_name2, course) values ('Quintilla', 'O''Breen', 'Piatti', '7ºRVI O');
insert into patients (name, last_name, last_name2, course) values ('Karalynn', 'Denerley', 'Vernall', '6ºGGS Z');
insert into patients (name, last_name, last_name2, course) values ('Guilbert', 'Spirritt', 'Cottingham', '0ºLKY H');
insert into patients (name, last_name, last_name2, course) values ('Beth', 'McVeagh', 'MacKnight', '1ºPKH H');
insert into patients (name, last_name, last_name2, course) values ('Darda', 'Szubert', 'Lapslie', '7ºLGM C');
insert into patients (name, last_name, last_name2, course) values ('Chryste', 'Leedal', 'Moneypenny', '5ºMNE A');
insert into patients (name, last_name, last_name2, course) values ('Conrade', 'Kinnach', 'Perchard', '7ºRRD K');
insert into patients (name, last_name, last_name2, course) values ('Thomasa', 'Fraulo', 'Capitano', '4ºPAP U');
insert into patients (name, last_name, last_name2, course) values ('Goraud', 'MacKintosh', 'Durkin', '7ºYXM F');
insert into patients (name, last_name, last_name2, course) values ('Georgi', 'Bompass', 'Roof', '2ºEWN L');
insert into patients (name, last_name, last_name2, course) values ('Cordey', 'Chrestien', 'Ince', '8ºPEO S');
insert into patients (name, last_name, last_name2, course) values ('Hasheem', 'Hargey', 'Whimp', '6ºQOT S');
insert into patients (name, last_name, last_name2, course) values ('Tate', 'Wankel', 'Greally', '8ºDRE H');
insert into patients (name, last_name, last_name2, course) values ('Cristina', 'O''Feeney', 'Pressman', '9ºIMP U');
insert into patients (name, last_name, last_name2, course) values ('Rorke', 'Trobe', 'Mault', '2ºXNE F');
insert into patients (name, last_name, last_name2, course) values ('Cyrille', 'Iannuzzelli', 'Thurber', '6ºCIJ K');
insert into patients (name, last_name, last_name2, course) values ('Greer', 'Moscon', 'Spriggin', '9ºYBU Z');
insert into patients (name, last_name, last_name2, course) values ('Maisey', 'Braben', 'Cruse', '8ºESL A');
insert into patients (name, last_name, last_name2, course) values ('Dennie', 'Wardlow', 'Hansberry', '0ºGNZ Y');
insert into patients (name, last_name, last_name2, course) values ('Jim', 'Barnett', 'Trewartha', '9ºTSP V');
insert into patients (name, last_name, last_name2, course) values ('Tara', 'Lorans', 'Boskell', '7ºRAD J');
insert into patients (name, last_name, last_name2, course) values ('Kania', 'Devey', 'Hackett', '4ºGIO D');
insert into patients (name, last_name, last_name2, course) values ('Abel', 'Davidson', 'Labell', '6ºJXF I');
insert into patients (name, last_name, last_name2, course) values ('Jessee', 'Burrells', 'Refford', '1ºOFJ O');
insert into patients (name, last_name, last_name2, course) values ('Connor', 'Ruckman', 'Velde', '7ºIOM N');
insert into patients (name, last_name, last_name2, course) values ('Dominic', 'Snalum', 'Minchi', '8ºGBD F');
insert into patients (name, last_name, last_name2, course) values ('Terese', 'Ditty', 'Knocker', '8ºXFD W');
insert into patients (name, last_name, last_name2, course) values ('Agathe', 'Whitchurch', 'MacAughtrie', '1ºDRP R');
insert into patients (name, last_name, last_name2, course) values ('Kerrie', 'Moggie', 'Guntrip', '9ºCBV R');
insert into patients (name, last_name, last_name2, course) values ('Anselm', 'A''Barrow', 'Hammond', '8ºINE O');
insert into patients (name, last_name, last_name2, course) values ('Huntlee', 'Sondon', 'Canceller', '0ºAAI G');
insert into patients (name, last_name, last_name2, course) values ('Marcelo', 'Hugle', 'Tolfrey', '4ºSPF F');
insert into patients (name, last_name, last_name2, course) values ('Delly', 'Grivori', 'Manklow', '1ºGTF D');
insert into patients (name, last_name, last_name2, course) values ('Byrle', 'Cushelly', 'Jentges', '3ºQJJ F');
insert into patients (name, last_name, last_name2, course) values ('Jorry', 'Spaice', 'Van Ross', '9ºPJA A');
insert into patients (name, last_name, last_name2, course) values ('Iosep', 'Jannasch', 'O''Beirne', '8ºHUK X');
insert into patients (name, last_name, last_name2, course) values ('Collete', 'Ketchen', 'Bentall', '4ºFBD Y');
insert into patients (name, last_name, last_name2, course) values ('Bronson', 'Cator', 'Pepperd', '8ºZLZ O');
insert into patients (name, last_name, last_name2, course) values ('Levi', 'Juliano', 'Oxlee', '4ºTDV E');
insert into patients (name, last_name, last_name2, course) values ('Batholomew', 'Burhill', 'Barnshaw', '9ºMLV R');
insert into patients (name, last_name, last_name2, course) values ('Ag', 'Gowen', 'Hansberry', '7ºWZL J');
insert into patients (name, last_name, last_name2, course) values ('Sean', 'Speke', 'Campione', '9ºSVD O');
insert into patients (name, last_name, last_name2, course) values ('Trumann', 'Gerber', 'Pordal', '3ºHZI C');
insert into patients (name, last_name, last_name2, course) values ('Dante', 'Hughes', 'Bradd', '0ºPXB M');
insert into patients (name, last_name, last_name2, course) values ('Lelah', 'Tullot', 'Yearby', '1ºXNC G');
insert into patients (name, last_name, last_name2, course) values ('Michele', 'Kighly', 'MacKill', '4ºOKL X');
insert into patients (name, last_name, last_name2, course) values ('Astrid', 'Scrannage', 'Akred', '7ºLJH D');
insert into patients (name, last_name, last_name2, course) values ('Wilden', 'Bamforth', 'Mathevon', '5ºAMI W');
insert into patients (name, last_name, last_name2, course) values ('Devan', 'Stoyle', 'Matterface', '7ºCNH V');
insert into patients (name, last_name, last_name2, course) values ('Monty', 'O''Hallihane', 'Langman', '0ºFEG Y');
insert into patients (name, last_name, last_name2, course) values ('Dur', 'Threader', 'MacMeeking', '7ºDWU G');
insert into patients (name, last_name, last_name2, course) values ('Stevie', 'Threlfall', 'Yuill', '6ºLIE P');
insert into patients (name, last_name, last_name2, course) values ('Randal', 'Neissen', 'Mariet', '6ºYKW Z');
insert into patients (name, last_name, last_name2, course) values ('Daniella', 'Guwer', 'Gallaway', '8ºQQT K');
insert into patients (name, last_name, last_name2, course) values ('Kristin', 'Reddie', 'Cunnah', '8ºSPG J');
insert into patients (name, last_name, last_name2, course) values ('Lorilee', 'Gonsalvo', 'Kraft', '3ºMMA T');
insert into patients (name, last_name, last_name2, course) values ('Gennie', 'Riply', 'Kimm', '8ºBTA H');
insert into patients (name, last_name, last_name2, course) values ('Constancy', 'Gilley', 'Berry', '0ºKPP W');
insert into patients (name, last_name, last_name2, course) values ('Nilson', 'Witcherley', 'Corry', '6ºKOT I');
insert into patients (name, last_name, last_name2, course) values ('Cybil', 'Wannop', 'Stranaghan', '4ºGKN C');
insert into patients (name, last_name, last_name2, course) values ('Marius', 'Willgoose', 'Firbank', '5ºAWQ D');
insert into patients (name, last_name, last_name2, course) values ('Rickie', 'Odam', 'Babonau', '6ºYYN I');
insert into patients (name, last_name, last_name2, course) values ('Gerhardine', 'Sigsworth', 'Rodolphe', '6ºPHQ C');
insert into patients (name, last_name, last_name2, course) values ('Freeman', 'Arber', 'Noar', '7ºSVQ S');
insert into patients (name, last_name, last_name2, course) values ('Carlynn', 'Sabie', 'Adolfsen', '4ºRAV F');
insert into patients (name, last_name, last_name2, course) values ('Sondra', 'Belden', 'Wandrey', '0ºJES O');
insert into patients (name, last_name, last_name2, course) values ('Gordy', 'Martygin', 'Hoffner', '8ºUPH W');
insert into patients (name, last_name, last_name2, course) values ('Nikolaus', 'Bedin', 'Clementet', '9ºWJX O');
insert into patients (name, last_name, last_name2, course) values ('Jo', 'Shirland', 'Jaulme', '9ºGXL T');
insert into patients (name, last_name, last_name2, course) values ('Bili', 'Groom', 'Philipeaux', '1ºNZX Q');
insert into patients (name, last_name, last_name2, course) values ('Talia', 'Struis', 'Reinmar', '6ºCWN E');
insert into patients (name, last_name, last_name2, course) values ('Angus', 'Tribble', 'Huxham', '5ºWPH N');
insert into patients (name, last_name, last_name2, course) values ('Iseabal', 'Andrys', 'Schimoni', '0ºHOJ U');
insert into patients (name, last_name, last_name2, course) values ('Vinny', 'Standeven', 'McFeat', '9ºOWE O');
insert into patients (name, last_name, last_name2, course) values ('Daron', 'Robiot', 'Klesl', '4ºGAH Q');
insert into patients (name, last_name, last_name2, course) values ('Corbin', 'Ivashechkin', 'Glave', '7ºSIH L');
insert into patients (name, last_name, last_name2, course) values ('Alecia', 'Batson', 'Trimming', '7ºTSI C');
insert into patients (name, last_name, last_name2, course) values ('Barr', 'Bryning', 'Carlyon', '2ºVMQ D');
insert into patients (name, last_name, last_name2, course) values ('Carolina', 'Nicolls', 'Tarquini', '0ºXPA B');
insert into patients (name, last_name, last_name2, course) values ('Sherwin', 'Maddie', 'Gascard', '6ºYHW Z');
insert into patients (name, last_name, last_name2, course) values ('Jefferey', 'Harburtson', 'Akroyd', '3ºDZQ V');
insert into patients (name, last_name, last_name2, course) values ('Charisse', 'O''Dooghaine', 'Geldart', '1ºQVT O');
insert into patients (name, last_name, last_name2, course) values ('Basilius', 'Jorry', 'McGruar', '5ºQRI E');
insert into patients (name, last_name, last_name2, course) values ('Moises', 'Jory', 'Gerssam', '4ºEWE L');
insert into patients (name, last_name, last_name2, course) values ('Boot', 'Kyston', 'Runacres', '4ºFKB C');
insert into patients (name, last_name, last_name2, course) values ('Edlin', 'Nowlan', 'Varvara', '5ºSER G');
insert into patients (name, last_name, last_name2, course) values ('Konstantin', 'Danforth', 'Whylie', '0ºUBS R');
insert into patients (name, last_name, last_name2, course) values ('Corella', 'Van der Mark', 'Jeannet', '9ºNOM D');
insert into patients (name, last_name, last_name2, course) values ('Pip', 'Haddrell', 'Strethill', '2ºQNT T');
insert into patients (name, last_name, last_name2, course) values ('Andreas', 'Tebbs', 'Sabathe', '5ºFEL M');
insert into patients (name, last_name, last_name2, course) values ('Hurleigh', 'Krolak', 'Shotboult', '5ºSZD F');
insert into patients (name, last_name, last_name2, course) values ('Benetta', 'Deinhard', 'Colbeck', '1ºQHO M');
insert into patients (name, last_name, last_name2, course) values ('Gawen', 'Bew', 'Ilyas', '0ºJRM N');
insert into patients (name, last_name, last_name2, course) values ('Coralyn', 'Devey', 'Fairbairn', '2ºBWT U');
insert into patients (name, last_name, last_name2, course) values ('Katheryn', 'Heibel', 'Hundey', '5ºWZW M');
insert into patients (name, last_name, last_name2, course) values ('Humbert', 'Dauney', 'Pantridge', '3ºVLW U');
insert into patients (name, last_name, last_name2, course) values ('Abbey', 'Feuell', 'Sutch', '0ºWUY C');
insert into patients (name, last_name, last_name2, course) values ('Teddie', 'Mcall', 'Gammett', '8ºRNB I');
insert into patients (name, last_name, last_name2, course) values ('Red', 'Mounsey', 'Hallford', '4ºNZS X');
insert into patients (name, last_name, last_name2, course) values ('Spense', 'Perdue', 'Ashley', '6ºRDF L');
insert into patients (name, last_name, last_name2, course) values ('Pierrette', 'Monckman', 'Gorbell', '1ºNQQ O');
insert into patients (name, last_name, last_name2, course) values ('Marla', 'Cross', 'Landes', '9ºISG F');
insert into patients (name, last_name, last_name2, course) values ('Mariam', 'Toomey', 'Jovis', '7ºZCF S');
insert into patients (name, last_name, last_name2, course) values ('Nye', 'Lundbech', 'Wewell', '8ºLHE X');
insert into patients (name, last_name, last_name2, course) values ('Mikey', 'Meneghi', 'Saich', '7ºMKL L');
insert into patients (name, last_name, last_name2, course) values ('Anett', 'Ivantyev', 'Halworth', '9ºMJX O');
insert into patients (name, last_name, last_name2, course) values ('Stevie', 'Pressman', 'Tames', '1ºNSH U');
insert into patients (name, last_name, last_name2, course) values ('Goldia', 'Hethron', 'Piffe', '0ºQCO A');
insert into patients (name, last_name, last_name2, course) values ('Arny', 'Orridge', 'Welch', '6ºHOV A');
insert into patients (name, last_name, last_name2, course) values ('Natale', 'Poltun', 'Liddington', '3ºCQA R');
insert into patients (name, last_name, last_name2, course) values ('Almira', 'Relton', 'Tather', '1ºEAX D');
insert into patients (name, last_name, last_name2, course) values ('Nolana', 'Foxon', 'Oliff', '2ºUIT J');
insert into patients (name, last_name, last_name2, course) values ('Chiarra', 'Ladd', 'Cutchie', '0ºNYS O');
insert into patients (name, last_name, last_name2, course) values ('Oberon', 'Nerney', 'Shemelt', '6ºJBD S');
insert into patients (name, last_name, last_name2, course) values ('Sharleen', 'Fuke', 'Drews', '6ºBJW S');
insert into patients (name, last_name, last_name2, course) values ('Raviv', 'Garaghan', 'Pleven', '3ºNYR J');
insert into patients (name, last_name, last_name2, course) values ('Rora', 'Coey', 'Udey', '5ºGOW J');
insert into patients (name, last_name, last_name2, course) values ('Bili', 'Sherbourne', 'Cornwell', '3ºXRB S');
insert into patients (name, last_name, last_name2, course) values ('Estrellita', 'Myles', 'Jakoubec', '0ºWKT V');
insert into patients (name, last_name, last_name2, course) values ('Joscelin', 'Gislebert', 'Brilon', '5ºBKR F');
insert into patients (name, last_name, last_name2, course) values ('Petunia', 'Rock', 'Pledge', '4ºQOD T');
insert into patients (name, last_name, last_name2, course) values ('Seline', 'Dettmar', 'Matthews', '8ºKXB Q');
insert into patients (name, last_name, last_name2, course) values ('Josephine', 'Bentham3', 'MacDermid', '4ºUMO A');
insert into patients (name, last_name, last_name2, course) values ('Lorens', 'Davidovici', 'Drews', '0ºJZB Q');
insert into patients (name, last_name, last_name2, course) values ('Chev', 'Swabey', 'Behn', '5ºPAC A');
insert into patients (name, last_name, last_name2, course) values ('Brunhilde', 'Slateford', 'Caccavale', '4ºPOO M');
insert into patients (name, last_name, last_name2, course) values ('Candi', 'Shawl', 'Hedon', '8ºIYE C');
insert into patients (name, last_name, last_name2, course) values ('Ram', 'Folcarelli', 'Dabernott', '0ºASQ M');
insert into patients (name, last_name, last_name2, course) values ('Felizio', 'Rayworth', 'Pykett', '0ºQAN C');
insert into patients (name, last_name, last_name2, course) values ('Parrnell', 'Wisher', 'Corzon', '8ºUXI E');
insert into patients (name, last_name, last_name2, course) values ('Oliy', 'Jenkyn', 'Brimham', '4ºFWU W');
insert into patients (name, last_name, last_name2, course) values ('Karrie', 'Oldroyd', 'Vasyushkhin', '0ºSVN D');
insert into patients (name, last_name, last_name2, course) values ('Ginevra', 'Varvell', 'Mullard', '0ºTRT O');
insert into patients (name, last_name, last_name2, course) values ('Iolande', 'Duffell', 'Jozsa', '7ºSYR H');
insert into patients (name, last_name, last_name2, course) values ('Glennie', 'Yalden', 'Currom', '1ºWED L');
insert into patients (name, last_name, last_name2, course) values ('Miguela', 'McKenzie', 'Callf', '4ºAUQ Y');
insert into patients (name, last_name, last_name2, course) values ('Lenka', 'Peatman', 'Peet', '7ºCMI U');
insert into patients (name, last_name, last_name2, course) values ('Perren', 'Dumingo', 'Scothorne', '7ºOGI M');
insert into patients (name, last_name, last_name2, course) values ('Orsola', 'Lerven', 'Scown', '9ºIXE G');
insert into patients (name, last_name, last_name2, course) values ('Dorothee', 'Lillistone', 'Bownass', '8ºRNK F');
insert into patients (name, last_name, last_name2, course) values ('Tomasina', 'Exer', 'Burner', '5ºKMS M');
insert into patients (name, last_name, last_name2, course) values ('Raffarty', 'Peirpoint', 'Delph', '7ºDGX X');
insert into patients (name, last_name, last_name2, course) values ('Merrilee', 'Fitchett', 'Flanner', '1ºMEK C');
insert into patients (name, last_name, last_name2, course) values ('Flory', 'Burch', 'Salleir', '4ºXWL K');
insert into patients (name, last_name, last_name2, course) values ('Fraze', 'Heal', 'Cunningham', '7ºBHT Y');
insert into patients (name, last_name, last_name2, course) values ('Ezequiel', 'Vanni', 'Crosetto', '1ºRXR K');
insert into patients (name, last_name, last_name2, course) values ('Ara', 'Gun', 'Pearman', '6ºNXX A');
insert into patients (name, last_name, last_name2, course) values ('Jacinta', 'Riteley', 'Firsby', '9ºGWL T');
insert into patients (name, last_name, last_name2, course) values ('Brad', 'Rebeiro', 'Petrussi', '0ºYNY Z');
insert into patients (name, last_name, last_name2, course) values ('Jorgan', 'Levecque', 'Featherstonhalgh', '2ºRDL T');
insert into patients (name, last_name, last_name2, course) values ('Ichabod', 'Spooner', 'Squeers', '0ºKLL C');
insert into patients (name, last_name, last_name2, course) values ('Marin', 'Berrill', 'Tripp', '8ºBYT M');
insert into patients (name, last_name, last_name2, course) values ('Ali', 'Tregale', 'Leupoldt', '9ºHFL Q');
insert into patients (name, last_name, last_name2, course) values ('Ronny', 'Pawellek', 'Philbin', '8ºACV E');
insert into patients (name, last_name, last_name2, course) values ('Milt', 'Haggas', 'Choppin', '3ºPOB M');
insert into patients (name, last_name, last_name2, course) values ('Ruperta', 'Muirhead', 'Cominetti', '5ºLUC C');
insert into patients (name, last_name, last_name2, course) values ('Talyah', 'Storey', 'Perelli', '0ºYBD N');
insert into patients (name, last_name, last_name2, course) values ('Laverne', 'Benne', 'Haddrell', '5ºAYQ K');
insert into patients (name, last_name, last_name2, course) values ('Beatrice', 'Swadlen', 'Asple', '1ºFFG E');
insert into patients (name, last_name, last_name2, course) values ('Theo', 'McCard', 'Hails', '8ºKUF N');
insert into patients (name, last_name, last_name2, course) values ('Christoffer', 'Kimmerling', 'Wickstead', '9ºPXM M');
insert into patients (name, last_name, last_name2, course) values ('Lizbeth', 'Bowater', 'Sigmund', '7ºQCN U');
insert into patients (name, last_name, last_name2, course) values ('Velma', 'Milington', 'Hatto', '9ºMDO M');
insert into patients (name, last_name, last_name2, course) values ('Ira', 'Dennick', 'Southby', '0ºMOT T');
insert into patients (name, last_name, last_name2, course) values ('Nowell', 'Sandys', 'Illiston', '8ºZOC E');
insert into patients (name, last_name, last_name2, course) values ('Hali', 'Ranson', 'Kreuzer', '6ºJBG N');
insert into patients (name, last_name, last_name2, course) values ('Boyd', 'MacCaughen', 'Tregust', '0ºHVX T');
insert into patients (name, last_name, last_name2, course) values ('Damon', 'Hoult', 'Chiswell', '9ºZSQ B');
insert into patients (name, last_name, last_name2, course) values ('Honoria', 'Schuricht', 'Ugoletti', '3ºNHU V');
insert into patients (name, last_name, last_name2, course) values ('Tamera', 'Durtnall', 'McOwen', '9ºOAY N');
insert into patients (name, last_name, last_name2, course) values ('Jenica', 'Harrop', 'Gooden', '0ºDLI Y');
insert into patients (name, last_name, last_name2, course) values ('Nevil', 'Tomkins', 'Crayton', '4ºLSY R');
insert into patients (name, last_name, last_name2, course) values ('Marco', 'Kemitt', 'Elwell', '3ºOZF J');
insert into patients (name, last_name, last_name2, course) values ('Cornell', 'MacConnell', 'Capelow', '4ºXCJ C');
insert into patients (name, last_name, last_name2, course) values ('Nike', 'Rafferty', 'Mableson', '6ºYMX L');
insert into patients (name, last_name, last_name2, course) values ('Neal', 'Conlon', 'Beamond', '5ºPRM K');
insert into patients (name, last_name, last_name2, course) values ('Gayler', 'Gabitis', 'Adamolli', '7ºXRI Y');
insert into patients (name, last_name, last_name2, course) values ('Gigi', 'Medley', 'Quadri', '0ºCSI B');
insert into patients (name, last_name, last_name2, course) values ('Veronika', 'Human', 'Brooksby', '9ºXPE X');
insert into patients (name, last_name, last_name2, course) values ('Sibylle', 'Trazzi', 'Cosgrive', '7ºBNY P');
insert into patients (name, last_name, last_name2, course) values ('Leyla', 'Rabjohn', 'Matonin', '9ºKDJ R');
insert into patients (name, last_name, last_name2, course) values ('Astrix', 'Soltan', 'Chessill', '1ºZOS O');
insert into patients (name, last_name, last_name2, course) values ('Lizzy', 'Laherty', 'Skill', '2ºNST T');
insert into patients (name, last_name, last_name2, course) values ('Dana', 'Jaggi', 'Bottoner', '7ºHVA C');
insert into patients (name, last_name, last_name2, course) values ('Dag', 'Luck', 'Creighton', '7ºXSP H');
insert into patients (name, last_name, last_name2, course) values ('Stevena', 'Wilfing', 'Nutbrown', '0ºDXP S');
insert into patients (name, last_name, last_name2, course) values ('Joly', 'Renshaw', 'Gilchriest', '2ºUMK L');
insert into patients (name, last_name, last_name2, course) values ('Barclay', 'Emmert', 'McGinty', '8ºRLA U');
insert into patients (name, last_name, last_name2, course) values ('Valle', 'Swithenby', 'Tattersall', '2ºGHU M');
insert into patients (name, last_name, last_name2, course) values ('Terrijo', 'Seedman', 'Petrulis', '9ºIVF K');
insert into patients (name, last_name, last_name2, course) values ('Reggie', 'Pappin', 'Imloch', '4ºGJZ F');
insert into patients (name, last_name, last_name2, course) values ('Janette', 'Willatts', 'Welbelove', '3ºFHB P');
insert into patients (name, last_name, last_name2, course) values ('Ede', 'Blackboro', 'Lownes', '3ºJKO Z');
insert into patients (name, last_name, last_name2, course) values ('Justina', 'Spondley', 'Haselden', '1ºUBZ N');
insert into patients (name, last_name, last_name2, course) values ('Evvie', 'Kacheler', 'Wimbridge', '8ºJUA Y');
insert into patients (name, last_name, last_name2, course) values ('Cross', 'Lage', 'Wyeld', '9ºUZN Y');
insert into patients (name, last_name, last_name2, course) values ('Sharron', 'Legge', 'Bonnor', '8ºZJI E');
insert into patients (name, last_name, last_name2, course) values ('Gabbi', 'Byass', 'Snodin', '1ºEDW G');
insert into patients (name, last_name, last_name2, course) values ('Edyth', 'Le Barre', 'Kaygill', '8ºMIB P');
insert into patients (name, last_name, last_name2, course) values ('Randie', 'Gever', 'Gimson', '6ºZYD N');
insert into patients (name, last_name, last_name2, course) values ('Archambault', 'Kinsley', 'Macilhench', '8ºCOI Q');
insert into patients (name, last_name, last_name2, course) values ('Glyn', 'Goolden', 'Grzegorczyk', '1ºMJT D');
insert into patients (name, last_name, last_name2, course) values ('Catlee', 'Acott', 'Smurfitt', '1ºHQG N');
insert into patients (name, last_name, last_name2, course) values ('Gilbertina', 'MacCaig', 'Tabb', '8ºQED J');
insert into patients (name, last_name, last_name2, course) values ('Sayre', 'Allebone', 'Coulson', '3ºGUV K');
insert into patients (name, last_name, last_name2, course) values ('Averil', 'Abramson', 'Berzin', '4ºQRL Z');
insert into patients (name, last_name, last_name2, course) values ('Tam', 'Lemerle', 'Feetham', '0ºOXL W');
insert into patients (name, last_name, last_name2, course) values ('Karry', 'Osment', 'Surgeon', '3ºQOX Z');
insert into patients (name, last_name, last_name2, course) values ('Hadria', 'Creaven', 'Gouldsmith', '0ºFTL B');
insert into patients (name, last_name, last_name2, course) values ('Gabi', 'Tynewell', 'Heak', '3ºDIN Y');
insert into patients (name, last_name, last_name2, course) values ('Thoma', 'Jamblin', 'Dragonette', '2ºYHI C');
insert into patients (name, last_name, last_name2, course) values ('Jerry', 'Adiscot', 'Witchell', '5ºUSC W');
insert into patients (name, last_name, last_name2, course) values ('Conni', 'Shrigley', 'Porch', '9ºXSZ U');
insert into patients (name, last_name, last_name2, course) values ('Sheelah', 'Sevier', 'Labrenz', '2ºLYS W');
insert into patients (name, last_name, last_name2, course) values ('Sapphira', 'Premble', 'Gladwell', '6ºOLM Q');
insert into patients (name, last_name, last_name2, course) values ('Raymond', 'Budget', 'McCrillis', '1ºBXK W');
insert into patients (name, last_name, last_name2, course) values ('Skippy', 'Baudichon', 'Redhouse', '9ºZTL Q');
insert into patients (name, last_name, last_name2, course) values ('Claudetta', 'Cripin', 'Bell', '8ºNTQ K');
insert into patients (name, last_name, last_name2, course) values ('Robinette', 'Cooley', 'Lasselle', '9ºIUP O');
insert into patients (name, last_name, last_name2, course) values ('Jacquenette', 'Gyde', 'Scargle', '6ºDSS T');
insert into patients (name, last_name, last_name2, course) values ('Simona', 'Aspital', 'Kiln', '6ºKKQ I');
insert into patients (name, last_name, last_name2, course) values ('Ian', 'Strong', 'Gissing', '3ºLSS U');
insert into patients (name, last_name, last_name2, course) values ('Klemens', 'Escala', 'Caush', '8ºEDE P');
insert into patients (name, last_name, last_name2, course) values ('Cazzie', 'Rivel', 'Joselevitch', '4ºWZN Q');
insert into patients (name, last_name, last_name2, course) values ('Ulrika', 'Lucchi', 'Drever', '8ºBHU L');
insert into patients (name, last_name, last_name2, course) values ('Solomon', 'Gutierrez', 'Dungate', '5ºLJV T');
insert into patients (name, last_name, last_name2, course) values ('Drugi', 'Mc Coughan', 'Wrenn', '8ºPMZ H');
insert into patients (name, last_name, last_name2, course) values ('Lorri', 'Farquhar', 'Chippendale', '1ºPZU X');
insert into patients (name, last_name, last_name2, course) values ('Jorey', 'Kalf', 'Caldecutt', '8ºANS D');
insert into patients (name, last_name, last_name2, course) values ('Tobit', 'Laughtisse', 'Korlat', '3ºXLB G');
insert into patients (name, last_name, last_name2, course) values ('Romain', 'Condit', 'Greenhill', '2ºBTF V');
insert into patients (name, last_name, last_name2, course) values ('Starlin', 'Cardiff', 'Copes', '9ºUHQ T');
insert into patients (name, last_name, last_name2, course) values ('Alexandr', 'Grenter', 'Burbudge', '0ºBWF E');
insert into patients (name, last_name, last_name2, course) values ('Stanislaw', 'Pyrke', 'Barnett', '2ºWLH C');
insert into patients (name, last_name, last_name2, course) values ('Brigitta', 'Lewzey', 'Castagna', '3ºCPE H');
insert into patients (name, last_name, last_name2, course) values ('Shane', 'Panons', 'Maureen', '0ºXUJ T');
insert into patients (name, last_name, last_name2, course) values ('Giulio', 'Linne', 'Calfe', '9ºDBE T');
insert into patients (name, last_name, last_name2, course) values ('Lucio', 'Sich', 'Blackaller', '0ºALQ G');
insert into patients (name, last_name, last_name2, course) values ('Jerad', 'Kaspar', 'Begin', '8ºMAB V');
insert into patients (name, last_name, last_name2, course) values ('Nessy', 'Kunrad', 'Heams', '4ºECS S');
insert into patients (name, last_name, last_name2, course) values ('Vivyan', 'Bariball', 'Dunguy', '3ºYIB Y');
insert into patients (name, last_name, last_name2, course) values ('Renato', 'Bugden', 'Fieldgate', '2ºWEC F');
insert into patients (name, last_name, last_name2, course) values ('Ingaborg', 'Strapp', 'Fluger', '2ºHNY L');
insert into patients (name, last_name, last_name2, course) values ('Carmencita', 'Stallan', 'McHan', '1ºPDH K');
insert into patients (name, last_name, last_name2, course) values ('Rebe', 'Dumblton', 'Barnsley', '0ºONK H');
insert into patients (name, last_name, last_name2, course) values ('Janine', 'Maggiore', 'Gammel', '1ºNWX F');
insert into patients (name, last_name, last_name2, course) values ('Lea', 'Mettetal', 'Murrum', '6ºXZU R');
insert into patients (name, last_name, last_name2, course) values ('Sibeal', 'Graundisson', 'Faucherand', '3ºJQM J');
insert into patients (name, last_name, last_name2, course) values ('Amelina', 'Vampouille', 'Tittletross', '7ºBBW N');
insert into patients (name, last_name, last_name2, course) values ('Glenda', 'Fay', 'Boylan', '6ºSIP M');
insert into patients (name, last_name, last_name2, course) values ('Lebbie', 'Barck', 'Yerrall', '4ºPLV E');
insert into patients (name, last_name, last_name2, course) values ('Jyoti', 'Timby', 'Realph', '1ºFVK A');
insert into patients (name, last_name, last_name2, course) values ('Ilene', 'Ingleby', 'McGilvra', '5ºTON A');
insert into patients (name, last_name, last_name2, course) values ('Cecilius', 'Hafner', 'Farnsworth', '4ºQAE T');
insert into patients (name, last_name, last_name2, course) values ('Hildegarde', 'Tutton', 'Waszkiewicz', '4ºRJT I');
insert into patients (name, last_name, last_name2, course) values ('Keane', 'Grunbaum', 'Tink', '0ºGVX X');
insert into patients (name, last_name, last_name2, course) values ('Lacey', 'Ablott', 'Carmont', '1ºOFD L');
insert into patients (name, last_name, last_name2, course) values ('Bealle', 'Firk', 'Deval', '3ºFRJ N');
insert into patients (name, last_name, last_name2, course) values ('Adam', 'Oiller', 'Phalp', '0ºNDA R');
insert into patients (name, last_name, last_name2, course) values ('Donnell', 'MacDermot', 'Guthrie', '1ºSGF F');
insert into patients (name, last_name, last_name2, course) values ('Archambault', 'Stede', 'O''Codihie', '5ºFCU Y');
insert into patients (name, last_name, last_name2, course) values ('Fallon', 'Radke', 'Lanceley', '5ºVTX X');
insert into patients (name, last_name, last_name2, course) values ('Shelagh', 'O''Growgane', 'Venturoli', '7ºIZD L');
insert into patients (name, last_name, last_name2, course) values ('Morley', 'Hearty', 'Mash', '5ºEWF P');
insert into patients (name, last_name, last_name2, course) values ('Gina', 'Lechmere', 'Kusick', '2ºLIR T');
insert into patients (name, last_name, last_name2, course) values ('Laurianne', 'Hansom', 'Hurlin', '2ºRIQ J');
insert into patients (name, last_name, last_name2, course) values ('Hendrick', 'Gummary', 'Hazelgreave', '1ºZWA F');
insert into patients (name, last_name, last_name2, course) values ('Olivia', 'Amberson', 'Farren', '8ºCRS C');
insert into patients (name, last_name, last_name2, course) values ('Terrance', 'Swalwell', 'Nestle', '7ºVUU Q');
insert into patients (name, last_name, last_name2, course) values ('Merissa', 'Paddock', 'Padillo', '9ºUJT O');
insert into patients (name, last_name, last_name2, course) values ('Theo', 'Vescovo', 'Wiltshaw', '3ºBCQ D');
insert into patients (name, last_name, last_name2, course) values ('Clovis', 'Murrhaupt', 'Forrest', '6ºSZG R');
insert into patients (name, last_name, last_name2, course) values ('Mimi', 'Parsley', 'Omond', '2ºLTQ U');
insert into patients (name, last_name, last_name2, course) values ('Yoshiko', 'Edgley', 'Sanson', '8ºRWJ S');
insert into patients (name, last_name, last_name2, course) values ('Malynda', 'Silverston', 'Shoppee', '3ºLGI F');
insert into patients (name, last_name, last_name2, course) values ('Noreen', 'Dewett', 'Helstrom', '4ºZCL M');
insert into patients (name, last_name, last_name2, course) values ('Iormina', 'Franca', 'Andreu', '3ºPZS F');
insert into patients (name, last_name, last_name2, course) values ('Leonardo', 'Synke', 'Raith', '9ºTTE Y');
insert into patients (name, last_name, last_name2, course) values ('Esther', 'McGahy', 'Cavozzi', '1ºAZP L');
insert into patients (name, last_name, last_name2, course) values ('Tamar', 'Marston', 'Spriddle', '4ºGLS C');
insert into patients (name, last_name, last_name2, course) values ('Madison', 'Queste', 'Neno', '0ºQUI Z');
insert into patients (name, last_name, last_name2, course) values ('Bryce', 'Rathke', 'Keasy', '9ºVEU G');
insert into patients (name, last_name, last_name2, course) values ('Raynard', 'Rugiero', 'Perring', '4ºMIJ M');
insert into patients (name, last_name, last_name2, course) values ('Whitney', 'Grieveson', 'Porrett', '6ºULX N');
insert into patients (name, last_name, last_name2, course) values ('Eyde', 'Phizaclea', 'Grunnell', '0ºJNW Y');
insert into patients (name, last_name, last_name2, course) values ('Trixie', 'Skeffington', 'D''Andrea', '7ºESH D');
insert into patients (name, last_name, last_name2, course) values ('Bernard', 'Ferney', 'Sargint', '9ºZJA P');
insert into patients (name, last_name, last_name2, course) values ('Fred', 'Neely', 'Claw', '9ºCFB Z');
insert into patients (name, last_name, last_name2, course) values ('Richmond', 'Longden', 'Pietzker', '6ºKLY Z');
insert into patients (name, last_name, last_name2, course) values ('Piotr', 'Pawnsford', 'Kuhl', '0ºLVR Q');
insert into patients (name, last_name, last_name2, course) values ('Gilly', 'Brinkworth', 'Opfer', '1ºDEA Y');
insert into patients (name, last_name, last_name2, course) values ('Richardo', 'Vasyutkin', 'Blonden', '2ºWCC E');
insert into patients (name, last_name, last_name2, course) values ('Vladimir', 'Hutchence', 'Habbergham', '8ºXIR O');
insert into patients (name, last_name, last_name2, course) values ('Kaia', 'Kiff', 'Sutherden', '1ºVJZ A');
insert into patients (name, last_name, last_name2, course) values ('Salvidor', 'Dopson', 'Cockland', '0ºTVE M');
insert into patients (name, last_name, last_name2, course) values ('Lee', 'MacKim', 'Flescher', '0ºUSF K');
insert into patients (name, last_name, last_name2, course) values ('Brandi', 'De''Ath', 'Endersby', '2ºDFV Q');
insert into patients (name, last_name, last_name2, course) values ('Joyann', 'Gooda', 'Goane', '5ºYSA V');
insert into patients (name, last_name, last_name2, course) values ('Alphard', 'Greatbank', 'Diment', '4ºQMT I');
insert into patients (name, last_name, last_name2, course) values ('Vivi', 'Lodin', 'Swetmore', '4ºJJT N');
insert into patients (name, last_name, last_name2, course) values ('Bobbie', 'Scholes', 'Mcsarry', '9ºLOB O');
insert into patients (name, last_name, last_name2, course) values ('Dun', 'Bingell', 'Linsley', '9ºTGM O');
insert into patients (name, last_name, last_name2, course) values ('Port', 'Kermannes', 'Tomblin', '7ºZFT T');
insert into patients (name, last_name, last_name2, course) values ('Retha', 'Baxendale', 'Doubrava', '3ºNKD V');
insert into patients (name, last_name, last_name2, course) values ('Drucill', 'Oswal', 'Pallister', '9ºSYJ H');
insert into patients (name, last_name, last_name2, course) values ('Hilliard', 'Eborall', 'Dyment', '1ºWKW F');
insert into patients (name, last_name, last_name2, course) values ('Olga', 'Ailward', 'Wooller', '6ºIXB R');
insert into patients (name, last_name, last_name2, course) values ('Rosabelle', 'Malan', 'McClune', '6ºQOX G');
insert into patients (name, last_name, last_name2, course) values ('Pandora', 'Clutram', 'Laurentin', '0ºBQI Z');
insert into patients (name, last_name, last_name2, course) values ('Carry', 'Willgoose', 'Meiningen', '5ºIGC R');
insert into patients (name, last_name, last_name2, course) values ('Brock', 'Tytterton', 'Longley', '1ºVDV D');
insert into patients (name, last_name, last_name2, course) values ('Anette', 'Fealy', 'Paskins', '0ºMNR A');
insert into patients (name, last_name, last_name2, course) values ('Rodolphe', 'Crosby', 'Kearford', '4ºSWQ C');
insert into patients (name, last_name, last_name2, course) values ('Eula', 'Hamman', 'Toppes', '8ºZBM X');
insert into patients (name, last_name, last_name2, course) values ('Davie', 'Treadgall', 'Torrent', '8ºOFT D');
insert into patients (name, last_name, last_name2, course) values ('Doralynn', 'Kinzel', 'Berrey', '2ºOZA B');
insert into patients (name, last_name, last_name2, course) values ('Archaimbaud', 'Bankhurst', 'Carling', '7ºYLE Z');
insert into patients (name, last_name, last_name2, course) values ('Deva', 'Butteris', 'Maker', '7ºUPZ Z');
insert into patients (name, last_name, last_name2, course) values ('Pru', 'Ainge', 'Underhill', '6ºOOL I');
insert into patients (name, last_name, last_name2, course) values ('Dyana', 'Downes', 'Barizeret', '8ºFKG Q');
insert into patients (name, last_name, last_name2, course) values ('Hillie', 'Placidi', 'Craze', '4ºNQH F');
insert into patients (name, last_name, last_name2, course) values ('Shelton', 'Headingham', 'Comettoi', '7ºOYE X');
insert into patients (name, last_name, last_name2, course) values ('Mariele', 'Freddi', 'Walaron', '0ºCHR I');
insert into patients (name, last_name, last_name2, course) values ('Gloriane', 'Pendre', 'Levesley', '4ºTII A');
insert into patients (name, last_name, last_name2, course) values ('Dodi', 'Copnar', 'de Najera', '0ºTZF Z');
insert into patients (name, last_name, last_name2, course) values ('Flor', 'Crumbie', 'Igoe', '6ºRUP Q');
insert into patients (name, last_name, last_name2, course) values ('Marcelle', 'Hunsworth', 'Brabin', '9ºISL D');
insert into patients (name, last_name, last_name2, course) values ('Glori', 'Laba', 'Sopper', '3ºXUN G');
insert into patients (name, last_name, last_name2, course) values ('Tana', 'Tabourin', 'Newbold', '5ºHEB J');
insert into patients (name, last_name, last_name2, course) values ('Torrin', 'Willment', 'Pelcheur', '3ºQRN N');
insert into patients (name, last_name, last_name2, course) values ('Joshia', 'Di Napoli', 'de Pinna', '2ºBEL Z');
insert into patients (name, last_name, last_name2, course) values ('Nedi', 'McWhin', 'O''Calleran', '3ºENV O');
insert into patients (name, last_name, last_name2, course) values ('Kristen', 'Aiers', 'Huett', '4ºHIK B');
insert into patients (name, last_name, last_name2, course) values ('Duke', 'Niblett', 'Iacopini', '2ºMXN R');
insert into patients (name, last_name, last_name2, course) values ('Ulrike', 'Wildin', 'Warner', '8ºPIC A');
insert into patients (name, last_name, last_name2, course) values ('Maddy', 'Mattson', 'Matussevich', '2ºDOL M');
insert into patients (name, last_name, last_name2, course) values ('Dulciana', 'Raith', 'Epsley', '5ºQRS R');
insert into patients (name, last_name, last_name2, course) values ('Wilma', 'July', 'Cadwaladr', '9ºXUW E');
insert into patients (name, last_name, last_name2, course) values ('Moses', 'Gatheral', 'Maskrey', '7ºFJB U');
insert into patients (name, last_name, last_name2, course) values ('Stanwood', 'Tookill', 'Slegg', '7ºJEI C');
insert into patients (name, last_name, last_name2, course) values ('Maud', 'Reignard', 'Fortoun', '3ºPTD T');
insert into patients (name, last_name, last_name2, course) values ('Viki', 'Teek', 'Jakel', '5ºTVI A');
insert into patients (name, last_name, last_name2, course) values ('Susannah', 'Dislee', 'Cardello', '1ºZJR J');
insert into patients (name, last_name, last_name2, course) values ('Harriette', 'Ezzle', 'Robiot', '1ºSXV K');
insert into patients (name, last_name, last_name2, course) values ('Collie', 'O''Nolan', 'Kruschev', '0ºOTD J');
insert into patients (name, last_name, last_name2, course) values ('Marius', 'Handling', 'Dron', '0ºEPD I');
insert into patients (name, last_name, last_name2, course) values ('Bridget', 'Emanuel', 'Kilminster', '0ºOUQ E');
insert into patients (name, last_name, last_name2, course) values ('Roberta', 'Ransbury', 'Cutten', '3ºXWM X');
insert into patients (name, last_name, last_name2, course) values ('Fredericka', 'Mell', 'Reynoldson', '4ºCHF T');
insert into patients (name, last_name, last_name2, course) values ('Tamera', 'Presnail', 'Shevill', '6ºPRS C');
insert into patients (name, last_name, last_name2, course) values ('Roberto', 'Sanpher', 'McCullen', '8ºADT I');
insert into patients (name, last_name, last_name2, course) values ('Nadia', 'Gawn', 'Suero', '3ºWDJ M');
insert into patients (name, last_name, last_name2, course) values ('Else', 'Liddy', 'Waszkiewicz', '9ºFBY E');
insert into patients (name, last_name, last_name2, course) values ('Leon', 'Screen', 'Blant', '3ºPLR T');
insert into patients (name, last_name, last_name2, course) values ('Corinna', 'Wagnerin', 'Bertwistle', '5ºVQG F');
insert into patients (name, last_name, last_name2, course) values ('Rahel', 'Bonniface', 'De Ambrosis', '8ºQGP K');
insert into patients (name, last_name, last_name2, course) values ('Chiquia', 'Ixer', 'Gloy', '6ºIYU E');
insert into patients (name, last_name, last_name2, course) values ('Kalle', 'Caplen', 'Gittings', '6ºVAU Q');
insert into patients (name, last_name, last_name2, course) values ('Corey', 'Edgeon', 'de la Tremoille', '8ºUFF J');
insert into patients (name, last_name, last_name2, course) values ('Bobbye', 'Chessum', 'Mountcastle', '5ºPBX J');
insert into patients (name, last_name, last_name2, course) values ('Zara', 'Warbrick', 'Mussen', '5ºEUG N');
insert into patients (name, last_name, last_name2, course) values ('Lorne', 'Elkins', 'Lygo', '2ºHTI S');
insert into patients (name, last_name, last_name2, course) values ('Lonnard', 'Dorber', 'Kellart', '5ºGJO A');
insert into patients (name, last_name, last_name2, course) values ('Humfrid', 'Culkin', 'Sharpley', '9ºQRD L');
insert into patients (name, last_name, last_name2, course) values ('Goldie', 'Compston', 'MacGown', '6ºAUO Z');
insert into patients (name, last_name, last_name2, course) values ('Filippa', 'Ivermee', 'Wolford', '4ºBCD K');
insert into patients (name, last_name, last_name2, course) values ('Natty', 'Flucker', 'Challenger', '8ºMIF F');
insert into patients (name, last_name, last_name2, course) values ('Marjorie', 'Skipworth', 'Tomsett', '0ºDVC Y');
insert into patients (name, last_name, last_name2, course) values ('Brigg', 'Rawle', 'Abramino', '4ºDNU Q');
insert into patients (name, last_name, last_name2, course) values ('Genevra', 'Gauld', 'Brandacci', '1ºCSR S');
insert into patients (name, last_name, last_name2, course) values ('Basile', 'Fowgies', 'Drakes', '5ºHMX V');
insert into patients (name, last_name, last_name2, course) values ('Davidde', 'Jacquemet', 'Castrillo', '0ºURY Q');
insert into patients (name, last_name, last_name2, course) values ('Anabal', 'Mallall', 'Oxby', '7ºFZG C');
insert into patients (name, last_name, last_name2, course) values ('Doralyn', 'Struss', 'Bridgeland', '4ºLIQ A');
insert into patients (name, last_name, last_name2, course) values ('Morie', 'Cargen', 'Barradell', '1ºEPT D');
insert into patients (name, last_name, last_name2, course) values ('Tanner', 'Crew', 'Dobney', '6ºVPV U');
insert into patients (name, last_name, last_name2, course) values ('Farley', 'Sheryne', 'Gilpillan', '3ºBJB D');
insert into patients (name, last_name, last_name2, course) values ('Norene', 'Shatliffe', 'Janway', '4ºSEU L');
insert into patients (name, last_name, last_name2, course) values ('Wenona', 'Tschiersch', 'Kinsey', '8ºANH A');
insert into patients (name, last_name, last_name2, course) values ('Barrie', 'Garret', 'O''Heffernan', '4ºULJ G');
insert into patients (name, last_name, last_name2, course) values ('Fanchon', 'Rasor', 'Wetherill', '5ºAHK G');
insert into patients (name, last_name, last_name2, course) values ('Gabby', 'Ganny', 'Pawlaczyk', '0ºQWF H');
insert into patients (name, last_name, last_name2, course) values ('Lillis', 'Ridulfo', 'Posvner', '1ºSSQ F');
insert into patients (name, last_name, last_name2, course) values ('Waylan', 'Goodfield', 'Crother', '9ºAVK V');
insert into patients (name, last_name, last_name2, course) values ('Charlean', 'Beardwell', 'MacKee', '1ºGRN F');
insert into patients (name, last_name, last_name2, course) values ('Kristel', 'Mate', 'Fursse', '3ºCBJ A');
insert into patients (name, last_name, last_name2, course) values ('Wilburt', 'Lissaman', 'Billingham', '7ºVHY W');
insert into patients (name, last_name, last_name2, course) values ('Kippy', 'Filpo', 'Skoughman', '8ºODJ Z');
insert into patients (name, last_name, last_name2, course) values ('Holmes', 'Tarbert', 'Darrigone', '5ºIDA O');
insert into patients (name, last_name, last_name2, course) values ('Leeland', 'Wailes', 'Cargenven', '9ºZFQ F');
insert into patients (name, last_name, last_name2, course) values ('Vickie', 'Cripwell', 'Richardes', '5ºSHB P');
insert into patients (name, last_name, last_name2, course) values ('Dorey', 'Ruzic', 'Anlay', '9ºTCR S');
insert into patients (name, last_name, last_name2, course) values ('Rahel', 'Aronovich', 'Gislebert', '8ºOHR M');
insert into patients (name, last_name, last_name2, course) values ('Gilberte', 'Farey', 'Hewins', '3ºERG U');
insert into patients (name, last_name, last_name2, course) values ('Calli', 'Jedrzejewsky', 'Juan', '7ºYSG V');
insert into patients (name, last_name, last_name2, course) values ('Michele', 'McIlvenna', 'Winchcomb', '5ºHFL W');
insert into patients (name, last_name, last_name2, course) values ('Estel', 'Duckels', 'Edland', '6ºDBS E');
insert into patients (name, last_name, last_name2, course) values ('Richard', 'Oakman', 'Jeanin', '2ºSUQ M');
insert into patients (name, last_name, last_name2, course) values ('Aylmar', 'Bawden', 'Benadette', '1ºZVA L');
insert into patients (name, last_name, last_name2, course) values ('Leeann', 'Turpie', 'Critchard', '1ºVXM E');
insert into patients (name, last_name, last_name2, course) values ('Tobie', 'Ludwig', 'Thody', '2ºJRQ G');
insert into patients (name, last_name, last_name2, course) values ('Renault', 'Heintz', 'Donat', '2ºFAL H');
insert into patients (name, last_name, last_name2, course) values ('Oralie', 'Fouracres', 'Nappin', '5ºEOR W');
insert into patients (name, last_name, last_name2, course) values ('Leola', 'Kimbell', 'Castellini', '7ºOCG H');
insert into patients (name, last_name, last_name2, course) values ('Raul', 'Trew', 'Tomaszewski', '5ºZSK O');
insert into patients (name, last_name, last_name2, course) values ('Ofilia', 'Sill', 'Boggis', '2ºTTB D');
insert into patients (name, last_name, last_name2, course) values ('Adina', 'Lackeye', 'Tribell', '0ºAJR N');
insert into patients (name, last_name, last_name2, course) values ('Ogden', 'Twitchings', 'Danhel', '6ºEIW X');
insert into patients (name, last_name, last_name2, course) values ('Enrika', 'Fidelli', 'Roylance', '3ºQSY W');
insert into patients (name, last_name, last_name2, course) values ('Ivonne', 'Estick', 'Ferrillio', '5ºIIM T');
insert into patients (name, last_name, last_name2, course) values ('Towney', 'Sautter', 'Vipan', '1ºOQT S');
insert into patients (name, last_name, last_name2, course) values ('Rolf', 'Van Oort', 'Colquhoun', '1ºTSO I');
insert into patients (name, last_name, last_name2, course) values ('Dawn', 'Barnsdale', 'Handes', '3ºLMI L');
insert into patients (name, last_name, last_name2, course) values ('Mona', 'Pietersen', 'Victor', '3ºRUB K');
insert into patients (name, last_name, last_name2, course) values ('Nigel', 'Franks', 'Ney', '0ºYLI A');
insert into patients (name, last_name, last_name2, course) values ('Emilee', 'McCobb', 'Danielsohn', '5ºVOT W');
insert into patients (name, last_name, last_name2, course) values ('Lucian', 'Spours', 'Shemelt', '7ºSPD X');
insert into patients (name, last_name, last_name2, course) values ('Bridie', 'Raden', 'Anger', '1ºZHI P');
insert into patients (name, last_name, last_name2, course) values ('Norton', 'Casillis', 'Eshmade', '0ºBOY O');
insert into patients (name, last_name, last_name2, course) values ('Armand', 'Stain', 'Gallacher', '2ºCMN G');
insert into patients (name, last_name, last_name2, course) values ('Gretel', 'O''Codihie', 'Worsall', '7ºJRK B');
insert into patients (name, last_name, last_name2, course) values ('Donall', 'Ambroziak', 'Gurnay', '7ºEXG E');
insert into patients (name, last_name, last_name2, course) values ('Angele', 'Dosdell', 'Antognelli', '6ºYLS O');
insert into patients (name, last_name, last_name2, course) values ('Base', 'Esherwood', 'Bodega', '8ºLPO Z');
insert into patients (name, last_name, last_name2, course) values ('Atlante', 'Groocock', 'Treversh', '1ºDXA R');
insert into patients (name, last_name, last_name2, course) values ('Kristo', 'Sucre', 'Mirfield', '1ºZCD M');
insert into patients (name, last_name, last_name2, course) values ('Shina', 'Withur', 'Mooney', '3ºGWI S');
insert into patients (name, last_name, last_name2, course) values ('Donnie', 'Checchetelli', 'MacEllen', '6ºOLB S');
insert into patients (name, last_name, last_name2, course) values ('Catherina', 'Bonde', 'Berndtssen', '3ºMRF M');
insert into patients (name, last_name, last_name2, course) values ('Shawn', 'Rois', 'Fitchet', '2ºEQH E');
insert into patients (name, last_name, last_name2, course) values ('Zandra', 'Lermouth', 'Jeffryes', '6ºJIG Y');
insert into patients (name, last_name, last_name2, course) values ('Trevor', 'Kynvin', 'Robrow', '1ºPLW X');
insert into patients (name, last_name, last_name2, course) values ('Kordula', 'Alelsandrowicz', 'Underdown', '0ºJJF F');
insert into patients (name, last_name, last_name2, course) values ('Thedric', 'Emanuelov', 'Searles', '4ºUSE C');
insert into patients (name, last_name, last_name2, course) values ('Jojo', 'Vivian', 'Snowdon', '9ºVZF K');
insert into patients (name, last_name, last_name2, course) values ('Alic', 'Stalley', 'Murdoch', '1ºJVL F');
insert into patients (name, last_name, last_name2, course) values ('Allix', 'Rutherforth', 'Lambert', '1ºPOQ W');
insert into patients (name, last_name, last_name2, course) values ('Gilda', 'Armit', 'Rolley', '0ºDEB F');
insert into patients (name, last_name, last_name2, course) values ('Ursala', 'Stebbing', 'Forrest', '5ºOTA T');
insert into patients (name, last_name, last_name2, course) values ('Penn', 'Bonifant', 'Gayne', '0ºIEL Q');
insert into patients (name, last_name, last_name2, course) values ('Murdoch', 'Belfit', 'Melburg', '4ºLPV I');
insert into patients (name, last_name, last_name2, course) values ('Morlee', 'Buckenham', 'Tissell', '8ºAYI C');
insert into patients (name, last_name, last_name2, course) values ('Hildegarde', 'Ashbey', 'Ishchenko', '5ºVLV K');
insert into patients (name, last_name, last_name2, course) values ('Breena', 'Malimoe', 'Osment', '8ºOTL R');
insert into patients (name, last_name, last_name2, course) values ('Jaimie', 'Mival', 'Aitkenhead', '3ºTDE S');
insert into patients (name, last_name, last_name2, course) values ('Sarena', 'Budnk', 'Bennetts', '0ºFRN I');
insert into patients (name, last_name, last_name2, course) values ('Sheffield', 'Burkinshaw', 'Andreuzzi', '4ºUHY O');
insert into patients (name, last_name, last_name2, course) values ('Nissie', 'Arden', 'Dossettor', '1ºPTF B');
insert into patients (name, last_name, last_name2, course) values ('Eunice', 'Pringour', 'London', '4ºSJM I');
insert into patients (name, last_name, last_name2, course) values ('Lorin', 'Allewell', 'Choke', '8ºAIR I');
insert into patients (name, last_name, last_name2, course) values ('Meredith', 'Roubay', 'Aistrop', '2ºTRM F');
insert into patients (name, last_name, last_name2, course) values ('Adelaida', 'Proud', 'Wallicker', '7ºXVX O');
insert into patients (name, last_name, last_name2, course) values ('Ange', 'Bristowe', 'Kibbel', '6ºAPR W');
insert into patients (name, last_name, last_name2, course) values ('Aeriell', 'Sterling', 'Writer', '0ºKYI C');
insert into patients (name, last_name, last_name2, course) values ('Halsy', 'Rollingson', 'Marusyak', '9ºJWW A');
insert into patients (name, last_name, last_name2, course) values ('Herschel', 'Siddens', 'Antoniades', '1ºHZF G');
insert into patients (name, last_name, last_name2, course) values ('Gabriell', 'Ivanin', 'Aitchison', '5ºBDQ P');
insert into patients (name, last_name, last_name2, course) values ('Jere', 'McGaw', 'Deverille', '6ºYTU I');
insert into patients (name, last_name, last_name2, course) values ('Ashleigh', 'Germon', 'McShane', '5ºENC D');
insert into patients (name, last_name, last_name2, course) values ('Laurene', 'Thickin', 'Allawy', '1ºMPI K');
insert into patients (name, last_name, last_name2, course) values ('Glory', 'Kirimaa', 'Ayers', '2ºYEC N');
insert into patients (name, last_name, last_name2, course) values ('Johnathon', 'Mundwell', 'Farrants', '8ºVYQ X');
insert into patients (name, last_name, last_name2, course) values ('Jasmina', 'Gain', 'McDougall', '3ºTDP F');
insert into patients (name, last_name, last_name2, course) values ('Minetta', 'Winson', 'Mennear', '8ºMOZ K');
insert into patients (name, last_name, last_name2, course) values ('Urson', 'Pizey', 'Whilde', '8ºUWV L');
insert into patients (name, last_name, last_name2, course) values ('Cecilio', 'Goodoune', 'Foat', '5ºJUG B');
insert into patients (name, last_name, last_name2, course) values ('Mendy', 'Sales', 'Winkell', '8ºOQI I');
insert into patients (name, last_name, last_name2, course) values ('Susi', 'Mager', 'Doick', '2ºNPW F');
insert into patients (name, last_name, last_name2, course) values ('Mariel', 'Sword', 'Salvidge', '4ºYDH Y');
insert into patients (name, last_name, last_name2, course) values ('Otes', 'Van Oort', 'Gerren', '4ºZTV C');
insert into patients (name, last_name, last_name2, course) values ('Valerie', 'Loffill', 'Maron', '8ºKWN M');
insert into patients (name, last_name, last_name2, course) values ('Zsa zsa', 'Whitmarsh', 'Arbuckel', '4ºBVR O');
insert into patients (name, last_name, last_name2, course) values ('Lorry', 'Bingell', 'Josefowicz', '5ºJLT E');
insert into patients (name, last_name, last_name2, course) values ('Jilly', 'Brettell', 'Moohan', '3ºFHM R');
insert into patients (name, last_name, last_name2, course) values ('Eugenius', 'Cordero', 'O''Geneay', '5ºDCE A');
insert into patients (name, last_name, last_name2, course) values ('Sterne', 'Shah', 'Davitashvili', '6ºENN Z');
insert into patients (name, last_name, last_name2, course) values ('Brynn', 'Kabsch', 'Southernwood', '6ºDNV N');
insert into patients (name, last_name, last_name2, course) values ('Tymothy', 'Bryning', 'Scoggans', '7ºPYF O');
insert into patients (name, last_name, last_name2, course) values ('Cherie', 'Everard', 'Kaser', '0ºMVB M');
insert into patients (name, last_name, last_name2, course) values ('Giacopo', 'Minger', 'Astbury', '0ºGOU M');
insert into patients (name, last_name, last_name2, course) values ('Killian', 'Buie', 'Stobbe', '9ºYLU U');
insert into patients (name, last_name, last_name2, course) values ('Aurthur', 'Yendall', 'Elger', '6ºXRH J');
insert into patients (name, last_name, last_name2, course) values ('Jude', 'Sobieski', 'Search', '3ºHEP I');
insert into patients (name, last_name, last_name2, course) values ('Alessandro', 'Bea', 'Austing', '5ºUGA D');
insert into patients (name, last_name, last_name2, course) values ('Erroll', 'Stanyer', 'Tassell', '5ºTBM H');
insert into patients (name, last_name, last_name2, course) values ('Shaylynn', 'Wrightson', 'Binnes', '1ºUMT X');
insert into patients (name, last_name, last_name2, course) values ('Matthiew', 'Smallbone', 'Osbaldstone', '5ºOUV G');
insert into patients (name, last_name, last_name2, course) values ('Ryan', 'Geibel', 'Udden', '4ºTAL H');
insert into patients (name, last_name, last_name2, course) values ('Carmelle', 'Hoston', 'Niche', '3ºYKV D');
insert into patients (name, last_name, last_name2, course) values ('Charla', 'Goldis', 'Kearle', '1ºOII E');
insert into patients (name, last_name, last_name2, course) values ('Dreddy', 'Gardener', 'Riddall', '5ºWBG R');
insert into patients (name, last_name, last_name2, course) values ('Aundrea', 'Sisse', 'Maker', '4ºSFE W');
insert into patients (name, last_name, last_name2, course) values ('Willis', 'Coombes', 'Martinec', '4ºLJX W');
insert into patients (name, last_name, last_name2, course) values ('Liana', 'Alasdair', 'Morehall', '6ºMKD Q');
insert into patients (name, last_name, last_name2, course) values ('Karrah', 'Towe', 'Bellay', '1ºDLH N');
insert into patients (name, last_name, last_name2, course) values ('Marci', 'Santorini', 'Melross', '4ºBVF Q');
insert into patients (name, last_name, last_name2, course) values ('Ashia', 'Baine', 'Biermatowicz', '4ºAFE N');
insert into patients (name, last_name, last_name2, course) values ('Jennie', 'Divina', 'Brocklebank', '6ºFKW P');
insert into patients (name, last_name, last_name2, course) values ('Saundra', 'Cracie', 'Ascraft', '2ºMAR Q');
insert into patients (name, last_name, last_name2, course) values ('Eberhard', 'Keepe', 'Godley', '5ºTRP M');
insert into patients (name, last_name, last_name2, course) values ('Irv', 'Batchan', 'Geeritz', '2ºHSJ Y');
insert into patients (name, last_name, last_name2, course) values ('Corry', 'Horsley', 'Campione', '9ºYHQ O');
insert into patients (name, last_name, last_name2, course) values ('Kane', 'Angier', 'Kermitt', '5ºFYA E');
insert into patients (name, last_name, last_name2, course) values ('Cybil', 'Mannock', 'Di Napoli', '6ºXIW Q');
insert into patients (name, last_name, last_name2, course) values ('Carole', 'Ravenscroftt', 'Tuite', '0ºDMF R');
insert into patients (name, last_name, last_name2, course) values ('Sonnnie', 'Tripean', 'Cord', '6ºFEQ Z');
insert into patients (name, last_name, last_name2, course) values ('Lynn', 'Ladlow', 'Scholig', '1ºAJH M');
insert into patients (name, last_name, last_name2, course) values ('Abigale', 'Kelcher', 'Glasscott', '3ºSRS A');
insert into patients (name, last_name, last_name2, course) values ('Margit', 'Sharrard', 'Durram', '2ºFXM L');
insert into patients (name, last_name, last_name2, course) values ('Hildegarde', 'O''Fielly', 'Normand', '1ºLEM E');
insert into patients (name, last_name, last_name2, course) values ('Elga', 'Manson', 'Widdowson', '2ºMEL F');
insert into patients (name, last_name, last_name2, course) values ('Ernestine', 'Wooller', 'Ault', '3ºVBK G');
insert into patients (name, last_name, last_name2, course) values ('Bastian', 'Hatrick', 'Manby', '2ºSUJ F');
insert into patients (name, last_name, last_name2, course) values ('Winston', 'Gwioneth', 'Rivel', '2ºIEM T');
insert into patients (name, last_name, last_name2, course) values ('Fan', 'McCarrison', 'Duffit', '2ºQRA W');
insert into patients (name, last_name, last_name2, course) values ('Basia', 'Thorn', 'Cordeux', '7ºHGU K');
insert into patients (name, last_name, last_name2, course) values ('Sascha', 'Doldon', 'Flann', '9ºVRF T');
insert into patients (name, last_name, last_name2, course) values ('Janelle', 'Tytler', 'McTeggart', '3ºDZS S');
insert into patients (name, last_name, last_name2, course) values ('Mart', 'Redmond', 'Caldecott', '8ºKVD O');
insert into patients (name, last_name, last_name2, course) values ('Zara', 'Kach', 'Cornillot', '3ºURD Y');
insert into patients (name, last_name, last_name2, course) values ('Brigg', 'Antoshin', 'Swatridge', '1ºDDX L');
insert into patients (name, last_name, last_name2, course) values ('Daniela', 'Woliter', 'Gettings', '2ºHPE L');
insert into patients (name, last_name, last_name2, course) values ('Florence', 'Pinnion', 'Cicculini', '7ºSVC N');
insert into patients (name, last_name, last_name2, course) values ('Deina', 'Pogson', 'Acres', '7ºMOU U');
insert into patients (name, last_name, last_name2, course) values ('Mahalia', 'Witts', 'Jeffrey', '3ºLNH R');
insert into patients (name, last_name, last_name2, course) values ('Frank', 'Mix', 'Scothorn', '6ºCGK K');
insert into patients (name, last_name, last_name2, course) values ('Lydia', 'Crandon', 'Stealfox', '6ºCAT X');
insert into patients (name, last_name, last_name2, course) values ('Meta', 'Medland', 'Moreno', '7ºKUS C');
insert into patients (name, last_name, last_name2, course) values ('Celestine', 'Slemming', 'Woodes', '7ºVNJ B');
insert into patients (name, last_name, last_name2, course) values ('Vevay', 'de la Valette Parisot', 'Feitosa', '9ºKPC D');
insert into patients (name, last_name, last_name2, course) values ('Allyce', 'Durram', 'Tredger', '0ºGZN C');
insert into patients (name, last_name, last_name2, course) values ('Hilly', 'Cartwright', 'Pitkeathly', '7ºIJF H');
insert into patients (name, last_name, last_name2, course) values ('Xever', 'Simonson', 'Gavrielly', '7ºWCS H');
insert into patients (name, last_name, last_name2, course) values ('Kitti', 'Cassels', 'Tremeer', '3ºNKP N');
insert into patients (name, last_name, last_name2, course) values ('Morry', 'Brychan', 'Blanket', '6ºWLA N');
insert into patients (name, last_name, last_name2, course) values ('Nan', 'Reignolds', 'Leas', '4ºTZU Y');
insert into patients (name, last_name, last_name2, course) values ('Bram', 'Binder', 'Tiddy', '2ºDKT Z');
insert into patients (name, last_name, last_name2, course) values ('Edmund', 'Newall', 'Lechelle', '3ºRXT B');
insert into patients (name, last_name, last_name2, course) values ('Elbertine', 'Pymm', 'Allaway', '2ºATS G');
insert into patients (name, last_name, last_name2, course) values ('Aubree', 'Handslip', 'Eick', '2ºVOF U');
insert into patients (name, last_name, last_name2, course) values ('Kelly', 'Junifer', 'Duncanson', '0ºQDA G');
insert into patients (name, last_name, last_name2, course) values ('Zelig', 'MacCostye', 'Raunds', '6ºUUJ C');
insert into patients (name, last_name, last_name2, course) values ('Desdemona', 'Elsby', 'Ollett', '3ºGEF R');
insert into patients (name, last_name, last_name2, course) values ('Florinda', 'Braunston', 'Learned', '2ºUHB C');
insert into patients (name, last_name, last_name2, course) values ('Felice', 'Convery', 'Larret', '7ºDMI C');
insert into patients (name, last_name, last_name2, course) values ('Lauren', 'Colicot', 'Kemell', '3ºPVO E');
insert into patients (name, last_name, last_name2, course) values ('Lauren', 'Lorryman', 'Kubecka', '2ºZER B');
insert into patients (name, last_name, last_name2, course) values ('Dyane', 'Totterdill', 'Smelley', '5ºHNK Z');
insert into patients (name, last_name, last_name2, course) values ('Urson', 'Boynes', 'Boxall', '7ºKJB P');
insert into patients (name, last_name, last_name2, course) values ('Nathalie', 'Boston', 'Lighton', '5ºJDR R');
insert into patients (name, last_name, last_name2, course) values ('Gianni', 'Digman', 'Hastings', '9ºMJQ X');
insert into patients (name, last_name, last_name2, course) values ('Arnuad', 'Genney', 'Cahn', '2ºNBT B');
insert into patients (name, last_name, last_name2, course) values ('Webster', 'Guiot', 'Pegler', '3ºSPN N');
insert into patients (name, last_name, last_name2, course) values ('Donielle', 'Cricket', 'Dedenham', '9ºPAA E');
insert into patients (name, last_name, last_name2, course) values ('Maure', 'Tamlett', 'Woolf', '0ºLIQ X');
insert into patients (name, last_name, last_name2, course) values ('Thaxter', 'Fawks', 'Bukac', '5ºVXY K');
insert into patients (name, last_name, last_name2, course) values ('Paxton', 'Clerk', 'Klampk', '5ºWIY P');
insert into patients (name, last_name, last_name2, course) values ('Pippa', 'Lindenberg', 'Rubartelli', '9ºZDF X');
insert into patients (name, last_name, last_name2, course) values ('Rodie', 'Creek', 'Kiraly', '9ºKPK Z');
insert into patients (name, last_name, last_name2, course) values ('Fremont', 'Vasyunichev', 'Azam', '7ºBJL S');
insert into patients (name, last_name, last_name2, course) values ('Eimile', 'Mikalski', 'Mebius', '2ºHQA R');
insert into patients (name, last_name, last_name2, course) values ('Brannon', 'Toffetto', 'Sawyer', '8ºITC X');
insert into patients (name, last_name, last_name2, course) values ('Ferdinand', 'Marzele', 'Urien', '3ºUFQ T');
insert into patients (name, last_name, last_name2, course) values ('Zacharia', 'Breslau', 'Deinhard', '1ºCCJ X');
insert into patients (name, last_name, last_name2, course) values ('Stefania', 'Kloska', 'Mengue', '4ºJTI Y');
insert into patients (name, last_name, last_name2, course) values ('Chaddy', 'Darracott', 'Benninck', '4ºLSN O');
insert into patients (name, last_name, last_name2, course) values ('Geordie', 'Stubs', 'McLoughlin', '0ºXHP I');
insert into patients (name, last_name, last_name2, course) values ('Darcy', 'Branney', 'Yarker', '5ºVNM T');
insert into patients (name, last_name, last_name2, course) values ('Cari', 'Parlet', 'Manchester', '4ºHIY T');
insert into patients (name, last_name, last_name2, course) values ('Gustavus', 'Bento', 'Colafate', '7ºYBX M');
insert into patients (name, last_name, last_name2, course) values ('Atalanta', 'Cristofolo', 'Alessandretti', '9ºXEM P');
insert into patients (name, last_name, last_name2, course) values ('Anthony', 'Grimmert', 'Smales', '6ºICT E');
insert into patients (name, last_name, last_name2, course) values ('Rafi', 'Bondesen', 'Nanuccioi', '6ºXEO B');
insert into patients (name, last_name, last_name2, course) values ('Jacquetta', 'Phetteplace', 'Lovelock', '1ºFDY X');
insert into patients (name, last_name, last_name2, course) values ('Leese', 'Skullet', 'Struther', '6ºMDD Y');
insert into patients (name, last_name, last_name2, course) values ('Duke', 'Schwander', 'Carlick', '2ºXTF V');
insert into patients (name, last_name, last_name2, course) values ('Marven', 'Jaksic', 'Presshaugh', '2ºCOJ N');
insert into patients (name, last_name, last_name2, course) values ('Kelsy', 'O''Currane', 'Rackstraw', '7ºYKS V');
insert into patients (name, last_name, last_name2, course) values ('Homere', 'Crittal', 'Wagstaffe', '9ºFZS P');
insert into patients (name, last_name, last_name2, course) values ('Karim', 'Allanson', 'Watson', '9ºYHP X');
insert into patients (name, last_name, last_name2, course) values ('Pamelina', 'Cousin', 'Hache', '5ºTWX V');
insert into patients (name, last_name, last_name2, course) values ('Chantal', 'Craney', 'Bagster', '5ºBGG A');
insert into patients (name, last_name, last_name2, course) values ('Esra', 'Hannent', 'Blasetti', '8ºIJQ B');
insert into patients (name, last_name, last_name2, course) values ('Reuven', 'Goodbarne', 'Kidby', '2ºDYS N');
insert into patients (name, last_name, last_name2, course) values ('Cary', 'Starton', 'Wipfler', '1ºUTF J');
insert into patients (name, last_name, last_name2, course) values ('Madlen', 'Mount', 'Cicci', '4ºNGY H');
insert into patients (name, last_name, last_name2, course) values ('Sylvia', 'Middlehurst', 'Pawelski', '2ºGBN B');
insert into patients (name, last_name, last_name2, course) values ('Miller', 'Hillum', 'Antonutti', '0ºEQZ K');
insert into patients (name, last_name, last_name2, course) values ('Lind', 'Fehely', 'Gudgeon', '8ºRHE G');
insert into patients (name, last_name, last_name2, course) values ('Madeleine', 'Chevins', 'Quaife', '3ºBIR C');
insert into patients (name, last_name, last_name2, course) values ('Dmitri', 'Vallance', 'Messingham', '8ºABT H');
insert into patients (name, last_name, last_name2, course) values ('Obidiah', 'Du Hamel', 'Hurren', '1ºTGL B');
insert into patients (name, last_name, last_name2, course) values ('Chevy', 'Charlewood', 'Maevela', '5ºUCT P');
insert into patients (name, last_name, last_name2, course) values ('Jereme', 'Allott', 'Cobbledick', '2ºPKC D');
insert into patients (name, last_name, last_name2, course) values ('Philippine', 'Laingmaid', 'Honywill', '7ºXFU O');
insert into patients (name, last_name, last_name2, course) values ('Cassey', 'Haskell', 'Rotherforth', '4ºXKL Y');
insert into patients (name, last_name, last_name2, course) values ('Hayley', 'Burghall', 'Ratcliffe', '8ºYSL B');
insert into patients (name, last_name, last_name2, course) values ('Mauricio', 'Kaming', 'Klarzynski', '4ºTNB S');
insert into patients (name, last_name, last_name2, course) values ('Charlie', 'Kennea', 'Raulstone', '8ºYQT J');
insert into patients (name, last_name, last_name2, course) values ('Alley', 'Thorne', 'Marchello', '9ºVXV X');
insert into patients (name, last_name, last_name2, course) values ('Austin', 'Muslim', 'Erington', '0ºHZW S');
insert into patients (name, last_name, last_name2, course) values ('Ethelind', 'Kneller', 'Lalevee', '9ºUWX M');
insert into patients (name, last_name, last_name2, course) values ('Prudy', 'Lemme', 'Schouthede', '1ºGHS F');
insert into patients (name, last_name, last_name2, course) values ('Roger', 'Prier', 'Earney', '8ºZEM P');
insert into patients (name, last_name, last_name2, course) values ('Farlee', 'Perigoe', 'Boobier', '2ºAPQ W');
insert into patients (name, last_name, last_name2, course) values ('Johannah', 'Langdridge', 'Westman', '0ºANB P');
insert into patients (name, last_name, last_name2, course) values ('Cos', 'Cottrill', 'Cadamy', '8ºIHK S');
insert into patients (name, last_name, last_name2, course) values ('Vitoria', 'Jakubowsky', 'Macura', '1ºIZG U');
insert into patients (name, last_name, last_name2, course) values ('Julie', 'Loades', 'Fonteyne', '9ºRMQ D');
insert into patients (name, last_name, last_name2, course) values ('Louise', 'Welsh', 'Guerola', '3ºMOA L');
insert into patients (name, last_name, last_name2, course) values ('Blane', 'Witham', 'Adamowicz', '5ºMXP V');
insert into patients (name, last_name, last_name2, course) values ('Dickie', 'Tomaszewicz', 'Partener', '9ºHQT G');
insert into patients (name, last_name, last_name2, course) values ('Sonny', 'Dinsdale', 'Alkin', '8ºZBU Q');
insert into patients (name, last_name, last_name2, course) values ('Dorie', 'Brecken', 'Girtin', '5ºFVQ K');
insert into patients (name, last_name, last_name2, course) values ('Griffith', 'Pasticznyk', 'Howells', '7ºLLF U');
insert into patients (name, last_name, last_name2, course) values ('Cecilla', 'Ballingal', 'Balsdon', '3ºFIY O');
insert into patients (name, last_name, last_name2, course) values ('Marika', 'Braams', 'Darycott', '5ºVXE K');
insert into patients (name, last_name, last_name2, course) values ('Emili', 'Michurin', 'Daws', '1ºELL A');
insert into patients (name, last_name, last_name2, course) values ('Brendon', 'Asmus', 'Alleyne', '4ºDJL D');
insert into patients (name, last_name, last_name2, course) values ('Fey', 'Aslum', 'Franseco', '6ºRXF M');
insert into patients (name, last_name, last_name2, course) values ('Ketti', 'Toffoletto', 'Ransbury', '0ºJTU R');
insert into patients (name, last_name, last_name2, course) values ('Nicoli', 'Carme', 'Quin', '5ºMXZ I');
insert into patients (name, last_name, last_name2, course) values ('Emmie', 'Postles', 'Dow', '9ºACX U');
insert into patients (name, last_name, last_name2, course) values ('Rodger', 'Lilley', 'Quarry', '9ºGUY I');
insert into patients (name, last_name, last_name2, course) values ('Sherry', 'Ohrt', 'Hiscoke', '8ºUVJ C');
insert into patients (name, last_name, last_name2, course) values ('Halimeda', 'Cubuzzi', 'Ogborn', '3ºYRY X');
insert into patients (name, last_name, last_name2, course) values ('Rodd', 'Cordery', 'Clemendet', '0ºVYU A');
insert into patients (name, last_name, last_name2, course) values ('Felicia', 'Benoiton', 'Hebblewaite', '8ºAMZ E');
insert into patients (name, last_name, last_name2, course) values ('Penni', 'Greenroad', 'Giametti', '7ºRVG J');
insert into patients (name, last_name, last_name2, course) values ('Anny', 'Bonnor', 'Cleere', '2ºXBS X');
insert into patients (name, last_name, last_name2, course) values ('Guido', 'Dwyer', 'Kohrsen', '5ºZGV M');
insert into patients (name, last_name, last_name2, course) values ('Edy', 'Rickert', 'Witz', '5ºAGU F');
insert into patients (name, last_name, last_name2, course) values ('Rivalee', 'Firidolfi', 'Mahomet', '3ºSDP T');
insert into patients (name, last_name, last_name2, course) values ('Flore', 'Feaveer', 'Haacker', '1ºVEM D');
insert into patients (name, last_name, last_name2, course) values ('Miranda', 'Pizer', 'Nockolds', '7ºFHC L');
insert into patients (name, last_name, last_name2, course) values ('Rora', 'Dilawey', 'Harly', '9ºANE N');
insert into patients (name, last_name, last_name2, course) values ('Janene', 'Brearty', 'Woolgar', '6ºCRK J');
insert into patients (name, last_name, last_name2, course) values ('Madelaine', 'Meagher', 'Kreuzer', '0ºQHT N');
insert into patients (name, last_name, last_name2, course) values ('Kimbell', 'Petruszka', 'Febre', '0ºABQ L');
insert into patients (name, last_name, last_name2, course) values ('Shirline', 'Tiffney', 'Fern', '4ºVLF J');
insert into patients (name, last_name, last_name2, course) values ('Rikki', 'Helling', 'Skep', '0ºFGF L');
insert into patients (name, last_name, last_name2, course) values ('Blondie', 'Portam', 'Mendes', '5ºHYD S');
insert into patients (name, last_name, last_name2, course) values ('Kristal', 'Briatt', 'Carroll', '1ºLNR U');
insert into patients (name, last_name, last_name2, course) values ('Opal', 'Squelch', 'Usher', '0ºBOO S');
insert into patients (name, last_name, last_name2, course) values ('Jerrie', 'Heck', 'Looby', '3ºNGZ W');
insert into patients (name, last_name, last_name2, course) values ('Florri', 'Dome', 'Vergine', '1ºEKG N');
insert into patients (name, last_name, last_name2, course) values ('Amby', 'Mitchely', 'Halgarth', '6ºYIM I');
insert into patients (name, last_name, last_name2, course) values ('Errol', 'Linger', 'Sidey', '5ºAXE F');
insert into patients (name, last_name, last_name2, course) values ('Marta', 'Chuck', 'Rennebach', '9ºDLO Y');
insert into patients (name, last_name, last_name2, course) values ('Beryl', 'Ruffli', 'Giffen', '2ºXGH V');
insert into patients (name, last_name, last_name2, course) values ('Hussein', 'Cozens', 'Wolfinger', '3ºJXK G');
insert into patients (name, last_name, last_name2, course) values ('Roxana', 'Prendergrass', 'Bello', '7ºUMK A');
insert into patients (name, last_name, last_name2, course) values ('Waiter', 'Peacher', 'Cannavan', '7ºTXL N');
insert into patients (name, last_name, last_name2, course) values ('Danya', 'Coleman', 'Libbis', '4ºNQM S');
insert into patients (name, last_name, last_name2, course) values ('Ardelis', 'Copnell', 'Mayes', '3ºYPT K');
insert into patients (name, last_name, last_name2, course) values ('Moreen', 'Shieldon', 'Cawood', '9ºYMS E');
insert into patients (name, last_name, last_name2, course) values ('Zerk', 'Blything', 'Lindenstrauss', '0ºSUI Q');
insert into patients (name, last_name, last_name2, course) values ('Dominik', 'Ellyatt', 'McNirlan', '8ºTZU F');
insert into patients (name, last_name, last_name2, course) values ('Winifred', 'McCullagh', 'Livard', '0ºFWA I');
insert into patients (name, last_name, last_name2, course) values ('Benny', 'Windeatt', 'Stowers', '3ºYUO E');
insert into patients (name, last_name, last_name2, course) values ('Pierce', 'Chiese', 'Hagergham', '6ºXYF S');
insert into patients (name, last_name, last_name2, course) values ('Brit', 'Pill', 'Igounet', '6ºCJG K');
insert into patients (name, last_name, last_name2, course) values ('Georgy', 'Shooter', 'Anning', '4ºIHQ Y');
insert into patients (name, last_name, last_name2, course) values ('Elaina', 'Tossell', 'Vidgen', '7ºYHQ T');
insert into patients (name, last_name, last_name2, course) values ('Broddy', 'Jarmaine', 'Blowick', '0ºODN F');
insert into patients (name, last_name, last_name2, course) values ('Dewey', 'Tejada', 'Fairpool', '4ºJDV Y');
insert into patients (name, last_name, last_name2, course) values ('Nadiya', 'Cristofano', 'Cymper', '9ºFHB X');
insert into patients (name, last_name, last_name2, course) values ('Arny', 'Doppler', 'Jannequin', '6ºZTI L');
insert into patients (name, last_name, last_name2, course) values ('Adda', 'Brimilcombe', 'McDonough', '3ºOAY S');
insert into patients (name, last_name, last_name2, course) values ('Brittne', 'MacNeilley', 'Bolf', '8ºYUK U');
insert into patients (name, last_name, last_name2, course) values ('Yves', 'Worsnup', 'Reely', '1ºGLV M');
insert into patients (name, last_name, last_name2, course) values ('Padraic', 'Melan', 'Dakers', '3ºNFU B');
insert into patients (name, last_name, last_name2, course) values ('Farley', 'Brumbie', 'Spurdens', '1ºKPU Z');
insert into patients (name, last_name, last_name2, course) values ('Tildi', 'Johansen', 'Lightbourn', '5ºZRL O');
insert into patients (name, last_name, last_name2, course) values ('Dorisa', 'Izat', 'Durker', '3ºXDE F');
insert into patients (name, last_name, last_name2, course) values ('Arvy', 'Merriott', 'Geistbeck', '9ºUMC R');
insert into patients (name, last_name, last_name2, course) values ('Michail', 'Robyns', 'Straughan', '5ºBCY B');
insert into patients (name, last_name, last_name2, course) values ('Patty', 'Gambles', 'Todeo', '9ºBKS O');
insert into patients (name, last_name, last_name2, course) values ('Dolorita', 'Thomann', 'Gethyn', '7ºWWN M');
insert into patients (name, last_name, last_name2, course) values ('Murial', 'Dulany', 'Boulde', '8ºIDN O');
insert into patients (name, last_name, last_name2, course) values ('Jessalin', 'Corley', 'Davage', '0ºCAN K');
insert into patients (name, last_name, last_name2, course) values ('Feliks', 'Hamlet', 'Vasyutichev', '4ºPDR K');
insert into patients (name, last_name, last_name2, course) values ('Nessa', 'Churms', 'Coolican', '7ºGYK I');
insert into patients (name, last_name, last_name2, course) values ('Merilee', 'Sinkins', 'Rapier', '3ºRYT L');
insert into patients (name, last_name, last_name2, course) values ('Jacquenetta', 'Tailby', 'Sedge', '5ºXXP C');
insert into patients (name, last_name, last_name2, course) values ('Maura', 'Skirvane', 'Wyeth', '7ºVTQ D');
insert into patients (name, last_name, last_name2, course) values ('Erda', 'Bambrick', 'Fidele', '4ºSRA L');
insert into patients (name, last_name, last_name2, course) values ('Yuma', 'Brown', 'Bartlam', '5ºOSV D');
insert into patients (name, last_name, last_name2, course) values ('Stanislaus', 'Brunotti', 'Yarnell', '0ºMUN H');
insert into patients (name, last_name, last_name2, course) values ('Abdul', 'Szymanski', 'Tongs', '0ºBTS M');
insert into patients (name, last_name, last_name2, course) values ('Rufe', 'Bleesing', 'Harlin', '8ºNYN X');
insert into patients (name, last_name, last_name2, course) values ('Gillian', 'Sindell', 'Seagood', '0ºWEC Z');
insert into patients (name, last_name, last_name2, course) values ('Rebe', 'Risom', 'Benedicto', '0ºUJA K');
insert into patients (name, last_name, last_name2, course) values ('Marci', 'Streeten', 'Bluschke', '9ºTPW S');
insert into patients (name, last_name, last_name2, course) values ('Mabelle', 'Kuller', 'Didsbury', '5ºPXL P');
insert into patients (name, last_name, last_name2, course) values ('Vivie', 'Leer', 'Muzzi', '7ºOWR I');
insert into patients (name, last_name, last_name2, course) values ('Boigie', 'Bellam', 'Sergant', '3ºSZV U');
insert into patients (name, last_name, last_name2, course) values ('Petrina', 'Shellcross', 'Kemshell', '9ºVBO Z');
insert into patients (name, last_name, last_name2, course) values ('Odele', 'Brickham', 'Cropp', '3ºWGT I');
insert into patients (name, last_name, last_name2, course) values ('Phedra', 'Kirman', 'Olenchenko', '4ºFZM M');
insert into patients (name, last_name, last_name2, course) values ('Talbot', 'Bugge', 'Barden', '2ºFYD T');
insert into patients (name, last_name, last_name2, course) values ('Nevsa', 'De la Perrelle', 'Ensley', '0ºJZQ R');
insert into patients (name, last_name, last_name2, course) values ('Birdie', 'Drohane', 'Bartolomucci', '6ºESB Y');
insert into patients (name, last_name, last_name2, course) values ('Jasmin', 'Kerfoot', 'Simacek', '5ºTXC Q');
insert into patients (name, last_name, last_name2, course) values ('Myrna', 'Acome', 'Holtum', '5ºHDL Y');
insert into patients (name, last_name, last_name2, course) values ('Ronny', 'Wellbelove', 'Airy', '5ºFMP V');
insert into patients (name, last_name, last_name2, course) values ('Lydon', 'Tuminelli', 'Maseres', '4ºTVW Q');
insert into patients (name, last_name, last_name2, course) values ('Bogey', 'Redshaw', 'Dalyiel', '6ºQYY A');
insert into patients (name, last_name, last_name2, course) values ('Kerrill', 'Dimbylow', 'Brundrett', '2ºGUT Y');
insert into patients (name, last_name, last_name2, course) values ('Fowler', 'Suatt', 'Foskin', '4ºFPU H');
insert into patients (name, last_name, last_name2, course) values ('Basile', 'Bloxholm', 'Kopje', '9ºHKW Z');
insert into patients (name, last_name, last_name2, course) values ('Jessee', 'Sewell', 'Ord', '2ºDOD O');
insert into patients (name, last_name, last_name2, course) values ('Allsun', 'Alibone', 'Umpleby', '2ºXOS M');
insert into patients (name, last_name, last_name2, course) values ('Mirabelle', 'Catlette', 'Randal', '0ºRLF W');
insert into patients (name, last_name, last_name2, course) values ('Valentine', 'Earle', 'Wincott', '2ºQSH N');
insert into patients (name, last_name, last_name2, course) values ('Kippie', 'Sussams', 'Rosensaft', '7ºGGD C');
insert into patients (name, last_name, last_name2, course) values ('Connie', 'Gegg', 'Groucutt', '2ºUYY H');
insert into patients (name, last_name, last_name2, course) values ('Christal', 'Millen', 'Botterill', '1ºPPR J');
insert into patients (name, last_name, last_name2, course) values ('Lorelle', 'Colthard', 'Havelin', '7ºHAH K');
insert into patients (name, last_name, last_name2, course) values ('Victoir', 'Attfield', 'Lucey', '8ºIHU H');
insert into patients (name, last_name, last_name2, course) values ('Jeanelle', 'Nurdin', 'Rany', '0ºFPE X');
insert into patients (name, last_name, last_name2, course) values ('Loy', 'Buie', 'Notman', '5ºYKS B');
insert into patients (name, last_name, last_name2, course) values ('Koressa', 'Veillard', 'Poate', '4ºPTR Z');
insert into patients (name, last_name, last_name2, course) values ('Jocelyn', 'Bremmell', 'Duesbury', '2ºNAC W');
insert into patients (name, last_name, last_name2, course) values ('Brett', 'Whenham', 'Kennagh', '3ºVNC C');
insert into patients (name, last_name, last_name2, course) values ('Lauren', 'Hampshire', 'MacChaell', '4ºRDB A');
insert into patients (name, last_name, last_name2, course) values ('Padraig', 'Spillane', 'Chaves', '2ºOQD M');
insert into patients (name, last_name, last_name2, course) values ('Prentice', 'Girardet', 'Synder', '5ºBLD K');
insert into patients (name, last_name, last_name2, course) values ('Nataniel', 'Sedgefield', 'Loyndon', '0ºZRG O');
insert into patients (name, last_name, last_name2, course) values ('Rufus', 'Zoppie', 'Hammill', '1ºENG Q');
insert into patients (name, last_name, last_name2, course) values ('Courtnay', 'Hatzar', 'Bullar', '3ºTVG W');
insert into patients (name, last_name, last_name2, course) values ('Axe', 'Frounks', 'McDermott-Row', '9ºFMB A');
insert into patients (name, last_name, last_name2, course) values ('Dean', 'Ebbett', 'Drawmer', '7ºMBG M');
insert into patients (name, last_name, last_name2, course) values ('Michal', 'Guyon', 'Cleere', '2ºLLA N');
insert into patients (name, last_name, last_name2, course) values ('Josefina', 'Dudney', 'Leitche', '0ºMBA Q');
insert into patients (name, last_name, last_name2, course) values ('Arielle', 'Abels', 'Wegenen', '1ºYVI J');
insert into patients (name, last_name, last_name2, course) values ('Phineas', 'Mushawe', 'Kohler', '0ºWHG W');
insert into patients (name, last_name, last_name2, course) values ('Way', 'Ubach', 'Dilworth', '1ºIYP V');
insert into patients (name, last_name, last_name2, course) values ('Angelia', 'Geram', 'Dulany', '2ºXPB R');
insert into patients (name, last_name, last_name2, course) values ('Alexina', 'Farmery', 'Scutter', '9ºTQJ A');
insert into patients (name, last_name, last_name2, course) values ('Horatius', 'Fiddyment', 'Tertre', '9ºYDB P');
insert into patients (name, last_name, last_name2, course) values ('Sela', 'Cleminshaw', 'Pepper', '9ºHSO M');
insert into patients (name, last_name, last_name2, course) values ('Marie', 'Cochet', 'Francescuzzi', '5ºQLC K');
insert into patients (name, last_name, last_name2, course) values ('Puff', 'Ridesdale', 'Runacres', '0ºAPG Y');
insert into patients (name, last_name, last_name2, course) values ('Sybilla', 'Trownson', 'Stockney', '1ºGKW L');
insert into patients (name, last_name, last_name2, course) values ('Indira', 'Theml', 'Whittet', '5ºDLN G');
insert into patients (name, last_name, last_name2, course) values ('Rani', 'Alloisi', 'Matz', '9ºPRK G');
insert into patients (name, last_name, last_name2, course) values ('Sally', 'Makeswell', 'Le Franc', '5ºKBH Z');
insert into patients (name, last_name, last_name2, course) values ('Florry', 'Grubb', 'Slessor', '5ºBAO W');
insert into patients (name, last_name, last_name2, course) values ('Erasmus', 'Adrain', 'D''Andrea', '1ºHLD N');
insert into patients (name, last_name, last_name2, course) values ('Yancy', 'Spong', 'Duffrie', '0ºVUU R');
insert into patients (name, last_name, last_name2, course) values ('Betty', 'Lamplugh', 'Moyne', '9ºQIU F');
insert into patients (name, last_name, last_name2, course) values ('Fabiano', 'Antonijevic', 'Mesias', '6ºFPL B');
insert into patients (name, last_name, last_name2, course) values ('Micheil', 'Eads', 'Bowe', '4ºVBQ J');
insert into patients (name, last_name, last_name2, course) values ('Paddie', 'Emerton', 'Lanney', '4ºWEO P');
insert into patients (name, last_name, last_name2, course) values ('Guillema', 'Meeks', 'Cudiff', '6ºVNW Y');
insert into patients (name, last_name, last_name2, course) values ('Jeffrey', 'Mayho', 'Britcher', '9ºYQC Q');
insert into patients (name, last_name, last_name2, course) values ('Royall', 'Osmint', 'Guinness', '7ºNHL N');
insert into patients (name, last_name, last_name2, course) values ('Leonidas', 'Gurys', 'Filipczak', '0ºLOY W');
insert into patients (name, last_name, last_name2, course) values ('Dominga', 'Sutherland', 'O''Shaughnessy', '6ºWVD Z');
insert into patients (name, last_name, last_name2, course) values ('Nolly', 'Baigent', 'MacVagh', '3ºZGQ Y');
insert into patients (name, last_name, last_name2, course) values ('Maxi', 'Aysik', 'Iveson', '0ºTAE E');
insert into patients (name, last_name, last_name2, course) values ('Germaine', 'Troy', 'Fitzpatrick', '7ºEPQ H');
insert into patients (name, last_name, last_name2, course) values ('Tamarra', 'Pavett', 'Sealeaf', '4ºKUX O');
insert into patients (name, last_name, last_name2, course) values ('Daffi', 'Le Cornu', 'Zaniolini', '3ºPGA D');
insert into patients (name, last_name, last_name2, course) values ('Humphrey', 'Josephsen', 'Threadkell', '3ºVRL V');
insert into patients (name, last_name, last_name2, course) values ('Astra', 'McDuall', 'Coldrick', '1ºDCN H');
insert into patients (name, last_name, last_name2, course) values ('Terencio', 'Bovingdon', 'Yerlett', '6ºWNW K');
insert into patients (name, last_name, last_name2, course) values ('Annora', 'Degoix', 'Stamp', '8ºEJB S');
insert into patients (name, last_name, last_name2, course) values ('Howey', 'Stickler', 'Windibank', '5ºLOR R');
insert into patients (name, last_name, last_name2, course) values ('Cazzie', 'Koppelmann', 'Ulyatt', '3ºHUQ T');
insert into patients (name, last_name, last_name2, course) values ('Lorant', 'Foale', 'Forber', '0ºHFF J');
insert into patients (name, last_name, last_name2, course) values ('Berry', 'Rosenhaupt', 'Haggis', '4ºTAI G');
insert into patients (name, last_name, last_name2, course) values ('Allyson', 'Sackey', 'Evemy', '2ºCRB D');
insert into patients (name, last_name, last_name2, course) values ('Greg', 'Cuckson', 'Sousa', '9ºXLN L');
insert into patients (name, last_name, last_name2, course) values ('Etheline', 'Meaney', 'Eddison', '9ºIJK N');
insert into patients (name, last_name, last_name2, course) values ('Ceil', 'Raithbie', 'Vampouille', '1ºRPT O');
insert into patients (name, last_name, last_name2, course) values ('Cash', 'McConigal', 'Gainsborough', '8ºYAX O');
insert into patients (name, last_name, last_name2, course) values ('Aloysia', 'Fobidge', 'Candlish', '5ºMCN V');
insert into patients (name, last_name, last_name2, course) values ('Georg', 'Standley', 'Orriss', '3ºAEM W');
insert into patients (name, last_name, last_name2, course) values ('Doria', 'Calcut', 'Crombleholme', '8ºBXG L');
insert into patients (name, last_name, last_name2, course) values ('Patton', 'Mungham', 'Dodle', '2ºOSO J');
insert into patients (name, last_name, last_name2, course) values ('Evvy', 'Pilbeam', 'Jobbins', '7ºOHG O');
insert into patients (name, last_name, last_name2, course) values ('Christal', 'Farington', 'Unwins', '9ºVSI F');
insert into patients (name, last_name, last_name2, course) values ('Liza', 'Lancett', 'Nassau', '1ºURK F');
insert into patients (name, last_name, last_name2, course) values ('Alec', 'Kaplan', 'Seeler', '1ºCJV Y');
insert into patients (name, last_name, last_name2, course) values ('Klara', 'Woolham', 'Twatt', '4ºSYQ W');
insert into patients (name, last_name, last_name2, course) values ('Karoline', 'Gronw', 'Iori', '7ºLBQ Y');
insert into patients (name, last_name, last_name2, course) values ('Laure', 'Woolens', 'Birtle', '6ºKXG U');
insert into patients (name, last_name, last_name2, course) values ('Benetta', 'Torregiani', 'Kemp', '9ºFLC S');
insert into patients (name, last_name, last_name2, course) values ('Karee', 'Bradly', 'Hubbock', '9ºSAP V');
insert into patients (name, last_name, last_name2, course) values ('Demott', 'Martland', 'Dearell', '6ºIPP S');
insert into patients (name, last_name, last_name2, course) values ('Vevay', 'Gitsham', 'Charrier', '3ºMMI Y');
insert into patients (name, last_name, last_name2, course) values ('Roberto', 'Blissett', 'Burdus', '8ºCSQ O');
insert into patients (name, last_name, last_name2, course) values ('Golda', 'Humpherson', 'Buckthought', '3ºNGQ Z');
insert into patients (name, last_name, last_name2, course) values ('Anica', 'Leftwich', 'Gopsell', '8ºIOB K');
insert into patients (name, last_name, last_name2, course) values ('Brewer', 'Hales', 'Lared', '7ºUZE W');
insert into patients (name, last_name, last_name2, course) values ('Zolly', 'Treasure', 'Aspey', '6ºEFU U');
insert into patients (name, last_name, last_name2, course) values ('Christopher', 'Mottershead', 'Shimwall', '0ºCOZ G');
insert into patients (name, last_name, last_name2, course) values ('Maynord', 'Wasiel', 'Hogsden', '2ºDKM D');
insert into patients (name, last_name, last_name2, course) values ('Al', 'Letteresse', 'Lawlance', '6ºAFP Z');
insert into patients (name, last_name, last_name2, course) values ('Romonda', 'Oller', 'Beswick', '2ºRYW N');
insert into patients (name, last_name, last_name2, course) values ('Darcee', 'Stanlick', 'Eberts', '4ºYCM V');
insert into patients (name, last_name, last_name2, course) values ('Conrad', 'McLise', 'Cuberley', '3ºAYK X');
insert into patients (name, last_name, last_name2, course) values ('Daryle', 'Christofides', 'Godar', '3ºEZK M');
insert into patients (name, last_name, last_name2, course) values ('Vida', 'Gaudin', 'Reidshaw', '5ºSWI G');
insert into patients (name, last_name, last_name2, course) values ('Aili', 'Lethbrig', 'Hefford', '6ºDGV H');
insert into patients (name, last_name, last_name2, course) values ('Tamra', 'Gude', 'Bogges', '8ºFTT R');
insert into patients (name, last_name, last_name2, course) values ('Carlin', 'Duckels', 'Cassimer', '0ºRGH C');
insert into patients (name, last_name, last_name2, course) values ('Milissent', 'Aston', 'Whitehurst', '6ºGDM G');
insert into patients (name, last_name, last_name2, course) values ('Garnette', 'Braddick', 'McCrum', '6ºSIC V');
insert into patients (name, last_name, last_name2, course) values ('Mimi', 'O''Regan', 'Horburgh', '6ºGAJ A');
insert into patients (name, last_name, last_name2, course) values ('Harlin', 'Humble', 'Quemby', '5ºORC X');
insert into patients (name, last_name, last_name2, course) values ('Pascal', 'Yukhnov', 'Ackerley', '7ºLTH X');
insert into patients (name, last_name, last_name2, course) values ('Erie', 'Charville', 'Westlake', '2ºKQY S');
insert into patients (name, last_name, last_name2, course) values ('Sher', 'Tribble', 'Forsaith', '7ºHWW Y');
insert into patients (name, last_name, last_name2, course) values ('Andeee', 'Tunmore', 'Dalston', '6ºIYO N');
insert into patients (name, last_name, last_name2, course) values ('Jo', 'Bungey', 'Janz', '6ºGDZ L');
insert into patients (name, last_name, last_name2, course) values ('Gerik', 'Castelluzzi', 'Ivons', '9ºKQG G');
insert into patients (name, last_name, last_name2, course) values ('Krystyna', 'Konneke', 'de Aguirre', '4ºGTJ X');
insert into patients (name, last_name, last_name2, course) values ('Gamaliel', 'Mercik', 'Steynor', '4ºJOW T');
insert into patients (name, last_name, last_name2, course) values ('Piper', 'Staite', 'Trillo', '2ºCIA C');
insert into patients (name, last_name, last_name2, course) values ('Lyn', 'Tremellan', 'Bambrugh', '6ºDTF H');
insert into patients (name, last_name, last_name2, course) values ('Addy', 'Dare', 'Slatten', '1ºEMA N');
insert into patients (name, last_name, last_name2, course) values ('Mervin', 'McVey', 'Reaney', '4ºAPQ H');
insert into patients (name, last_name, last_name2, course) values ('Hoebart', 'Shire', 'Molloy', '4ºCRM P');
insert into patients (name, last_name, last_name2, course) values ('Opaline', 'O''Cassidy', 'Vizor', '9ºZWI A');
insert into patients (name, last_name, last_name2, course) values ('Claribel', 'Brereton', 'Gidney', '7ºPHT K');
insert into patients (name, last_name, last_name2, course) values ('Dunn', 'Brabin', 'Gowry', '4ºCDO N');
insert into patients (name, last_name, last_name2, course) values ('Maryrose', 'Pridham', 'Norwell', '2ºHKH E');
insert into patients (name, last_name, last_name2, course) values ('Lisbeth', 'Pulfer', 'Ferriere', '3ºLXT X');
insert into patients (name, last_name, last_name2, course) values ('Wilbur', 'Doggerell', 'Tarling', '3ºIBP G');
insert into patients (name, last_name, last_name2, course) values ('Ferdinande', 'Gaynor', 'Hedau', '1ºNRS A');
insert into patients (name, last_name, last_name2, course) values ('Osgood', 'Pallas', 'Climar', '1ºOVW T');
insert into patients (name, last_name, last_name2, course) values ('Hedwig', 'Margetts', 'Celli', '6ºGIG A');
insert into patients (name, last_name, last_name2, course) values ('Marya', 'Kelshaw', 'Meconi', '1ºLME B');
insert into patients (name, last_name, last_name2, course) values ('Gabey', 'Maliffe', 'Bartels', '5ºYUQ P');
insert into patients (name, last_name, last_name2, course) values ('Gena', 'Asprey', 'Manz', '7ºRTE V');
insert into patients (name, last_name, last_name2, course) values ('Roi', 'Upstone', 'Coppin', '6ºNEV A');
insert into patients (name, last_name, last_name2, course) values ('Caryn', 'Broadey', 'Piddlehinton', '0ºNNH B');
insert into patients (name, last_name, last_name2, course) values ('Bonnie', 'Winter', 'Grewer', '3ºQSL A');
insert into patients (name, last_name, last_name2, course) values ('Agace', 'Grastye', 'Raunds', '0ºBAS P');
insert into patients (name, last_name, last_name2, course) values ('Grannie', 'Le Galle', 'Ivey', '7ºUTA S');
insert into patients (name, last_name, last_name2, course) values ('Marv', 'Cromly', 'Akast', '5ºBUB M');
insert into patients (name, last_name, last_name2, course) values ('Chelsey', 'Chate', 'Merricks', '9ºCOX Y');
insert into patients (name, last_name, last_name2, course) values ('Margaretha', 'Girod', 'Emlen', '3ºJHK X');
insert into patients (name, last_name, last_name2, course) values ('Murial', 'Petrushka', 'Antat', '2ºDTM R');
insert into patients (name, last_name, last_name2, course) values ('Una', 'Orrick', 'Schelle', '1ºWVG D');
insert into patients (name, last_name, last_name2, course) values ('Eliot', 'Dionis', 'Speight', '5ºQKG J');
insert into patients (name, last_name, last_name2, course) values ('Les', 'Fatharly', 'Hounihan', '1ºMKA C');
insert into patients (name, last_name, last_name2, course) values ('Flory', 'Ambrosetti', 'Margram', '0ºXPF D');
insert into patients (name, last_name, last_name2, course) values ('Selia', 'Dreye', 'Graham', '9ºREW R');
insert into patients (name, last_name, last_name2, course) values ('Felizio', 'Terrazzo', 'Blakeden', '6ºROM X');
insert into patients (name, last_name, last_name2, course) values ('Alberik', 'Quare', 'Lettuce', '3ºEYQ Y');
insert into patients (name, last_name, last_name2, course) values ('Alica', 'Kerrigan', 'Wardle', '6ºTXV M');
insert into patients (name, last_name, last_name2, course) values ('Row', 'Gogerty', 'Barber', '2ºYJE X');
insert into patients (name, last_name, last_name2, course) values ('Ellene', 'Maitland', 'McGarva', '5ºLYG P');
insert into patients (name, last_name, last_name2, course) values ('Ardath', 'Lanmeid', 'Legat', '8ºRUA F');
insert into patients (name, last_name, last_name2, course) values ('Sibyl', 'Tomblett', 'Matterface', '2ºZCR M');
insert into patients (name, last_name, last_name2, course) values ('Yovonnda', 'Hartop', 'Caswall', '1ºIDZ S');
insert into patients (name, last_name, last_name2, course) values ('Binni', 'MacCome', 'Johnson', '8ºSIC E');
insert into patients (name, last_name, last_name2, course) values ('Wayne', 'Cutress', 'Tott', '6ºIGO A');
insert into patients (name, last_name, last_name2, course) values ('Hubie', 'Galgey', 'Land', '4ºYYV F');
insert into patients (name, last_name, last_name2, course) values ('Boot', 'Colton', 'Gribbin', '1ºWZO F');
insert into patients (name, last_name, last_name2, course) values ('Ina', 'Von Welden', 'Blackadder', '9ºYEB K');
insert into patients (name, last_name, last_name2, course) values ('Jenica', 'Ullyott', 'Jacmar', '5ºHSF H');
insert into patients (name, last_name, last_name2, course) values ('Janith', 'Tucsell', 'Furzey', '1ºJGC U');
insert into patients (name, last_name, last_name2, course) values ('Lucio', 'Sillitoe', 'Brackenridge', '0ºZXZ A');
insert into patients (name, last_name, last_name2, course) values ('Delmer', 'Sand', 'Spittal', '1ºUOJ O');
insert into patients (name, last_name, last_name2, course) values ('Jeth', 'Woehler', 'Pinck', '6ºJVK X');
insert into patients (name, last_name, last_name2, course) values ('Leeland', 'Rodliff', 'Doblin', '0ºGEG Q');
insert into patients (name, last_name, last_name2, course) values ('Paco', 'Wagner', 'Redmille', '1ºDZV Y');
insert into patients (name, last_name, last_name2, course) values ('Dunn', 'Brixey', 'Settle', '2ºVTD B');
insert into patients (name, last_name, last_name2, course) values ('Leighton', 'Leander', 'Leng', '8ºQNN Q');
insert into patients (name, last_name, last_name2, course) values ('Jo ann', 'Hemshall', 'Hamil', '2ºFVU V');
insert into patients (name, last_name, last_name2, course) values ('Beck', 'Casari', 'Lownes', '1ºGPX G');
insert into patients (name, last_name, last_name2, course) values ('Fredrika', 'Glave', 'Jacqueminot', '3ºKDN T');
insert into patients (name, last_name, last_name2, course) values ('Rhonda', 'Comberbeach', 'Havesides', '4ºBFH C');
insert into patients (name, last_name, last_name2, course) values ('Marsiella', 'Standall', 'Amberson', '0ºLYL X');
insert into patients (name, last_name, last_name2, course) values ('Haley', 'Gawn', 'Houlridge', '7ºHXW I');
insert into patients (name, last_name, last_name2, course) values ('Gaile', 'Sewall', 'Brun', '5ºOQJ M');
insert into patients (name, last_name, last_name2, course) values ('Gail', 'Sherbourne', 'Knott', '5ºRZZ Q');
insert into patients (name, last_name, last_name2, course) values ('Valaree', 'Hrynczyk', 'Scoines', '0ºWDQ P');
insert into patients (name, last_name, last_name2, course) values ('Elisa', 'Pouck', 'Lutas', '9ºYYA F');
insert into patients (name, last_name, last_name2, course) values ('Dionis', 'Fussell', 'Dobinson', '3ºAGQ F');
insert into patients (name, last_name, last_name2, course) values ('Coralie', 'Extill', 'Crottagh', '6ºYWU M');
insert into patients (name, last_name, last_name2, course) values ('Tallou', 'Kybbye', 'Ravelus', '9ºHSF V');
insert into patients (name, last_name, last_name2, course) values ('Baird', 'Koop', 'Behninck', '4ºIVF N');
insert into patients (name, last_name, last_name2, course) values ('Fowler', 'Ponton', 'MacAloren', '0ºMTG U');
insert into patients (name, last_name, last_name2, course) values ('Ravid', 'Nast', 'Rainy', '9ºKRA P');
insert into patients (name, last_name, last_name2, course) values ('Basil', 'Passman', 'Gretham', '6ºJTH B');
insert into patients (name, last_name, last_name2, course) values ('Corissa', 'Maior', 'Mattiassi', '4ºTOD D');
insert into patients (name, last_name, last_name2, course) values ('Curry', 'Iacobassi', 'Cobb', '5ºSKF G');
insert into patients (name, last_name, last_name2, course) values ('Rachael', 'Ridsdale', 'Mushrow', '4ºPFO T');
insert into patients (name, last_name, last_name2, course) values ('Durand', 'Benoi', 'Collishaw', '8ºUHF N');
insert into patients (name, last_name, last_name2, course) values ('Nicolle', 'Subhan', 'Springthorp', '4ºMPB C');
insert into patients (name, last_name, last_name2, course) values ('Doris', 'Shickle', 'Leathlay', '5ºLZI M');
insert into patients (name, last_name, last_name2, course) values ('Trista', 'Bransom', 'Luquet', '1ºTXD U');
insert into patients (name, last_name, last_name2, course) values ('Dolph', 'Dudenie', 'Ciccarello', '1ºOIL A');
insert into patients (name, last_name, last_name2, course) values ('Lenore', 'Brumfield', 'Pappin', '0ºJVY N');
insert into patients (name, last_name, last_name2, course) values ('Emalia', 'Hambrook', 'Bambrugh', '0ºJKY S');
insert into patients (name, last_name, last_name2, course) values ('Paulina', 'Ivankov', 'Maggiori', '7ºVEG Z');
insert into patients (name, last_name, last_name2, course) values ('Luelle', 'Euesden', 'Kenningley', '6ºZZW W');
insert into patients (name, last_name, last_name2, course) values ('Templeton', 'Service', 'Seville', '8ºQIN Y');
insert into patients (name, last_name, last_name2, course) values ('Orland', 'Midford', 'Eles', '0ºSXL W');
insert into patients (name, last_name, last_name2, course) values ('Garek', 'Pharoah', 'Vain', '6ºRSO D');
insert into patients (name, last_name, last_name2, course) values ('Georgeanna', 'Ramel', 'Levicount', '6ºRUU C');
insert into patients (name, last_name, last_name2, course) values ('Ric', 'Gyurko', 'Connue', '3ºECQ K');
insert into patients (name, last_name, last_name2, course) values ('Antonius', 'Gorse', 'Coskerry', '2ºNLK F');
insert into patients (name, last_name, last_name2, course) values ('Kenyon', 'Dei', 'Stayt', '3ºTZI L');
insert into patients (name, last_name, last_name2, course) values ('Edvard', 'Riddington', 'Doswell', '0ºDYC K');
insert into patients (name, last_name, last_name2, course) values ('Darcy', 'Ochiltree', 'Fannon', '3ºXJM W');
insert into patients (name, last_name, last_name2, course) values ('Amye', 'Rhymes', 'Scroggins', '5ºEJK X');
insert into patients (name, last_name, last_name2, course) values ('Yasmin', 'Earney', 'Jeeves', '8ºWMD S');
insert into patients (name, last_name, last_name2, course) values ('Gabrielle', 'Willowby', 'Squelch', '1ºFVM O');
insert into patients (name, last_name, last_name2, course) values ('Matelda', 'Atkirk', 'Grzeszczak', '4ºKQZ I');
insert into patients (name, last_name, last_name2, course) values ('Rebecka', 'Lille', 'Guilleton', '2ºTFP R');
insert into patients (name, last_name, last_name2, course) values ('Cirilo', 'Lightwing', 'Marquet', '7ºMLI F');
insert into patients (name, last_name, last_name2, course) values ('Billy', 'Fuzzey', 'Laffranconi', '7ºXTJ O');
insert into patients (name, last_name, last_name2, course) values ('Leesa', 'Febre', 'Watkins', '6ºMFB J');
insert into patients (name, last_name, last_name2, course) values ('Winna', 'McCathay', 'Greet', '5ºQGS Y');
insert into patients (name, last_name, last_name2, course) values ('Lyn', 'Tidbury', 'McKerron', '8ºXQD J');
insert into patients (name, last_name, last_name2, course) values ('Louise', 'Vials', 'Clay', '6ºGSS P');
insert into patients (name, last_name, last_name2, course) values ('Sergent', 'Harral', 'Coneron', '6ºNDK N');
insert into patients (name, last_name, last_name2, course) values ('Lem', 'Jouaneton', 'Dener', '0ºMQX H');
insert into patients (name, last_name, last_name2, course) values ('Nancie', 'Ledrun', 'Knappett', '1ºTXS R');
insert into patients (name, last_name, last_name2, course) values ('Marj', 'Wildman', 'Grassett', '2ºPPL K');
insert into patients (name, last_name, last_name2, course) values ('Layla', 'Patient', 'Gillatt', '6ºNEN Z');
insert into patients (name, last_name, last_name2, course) values ('Gale', 'Wisdom', 'Persse', '6ºDOS F');
insert into patients (name, last_name, last_name2, course) values ('Lyssa', 'Roller', 'Sabie', '2ºRXG I');
insert into patients (name, last_name, last_name2, course) values ('Daven', 'Thackeray', 'Lornsen', '8ºWPW U');
insert into patients (name, last_name, last_name2, course) values ('Zenia', 'Phlippsen', 'Pitcock', '3ºAQV P');
insert into patients (name, last_name, last_name2, course) values ('Lurleen', 'Pilkinton', 'Chicco', '9ºEAV M');
insert into patients (name, last_name, last_name2, course) values ('Rodrique', 'Wanderschek', 'Curman', '5ºSFH Q');
insert into patients (name, last_name, last_name2, course) values ('Bobby', 'Alyonov', 'Dymocke', '9ºZGW S');
insert into patients (name, last_name, last_name2, course) values ('Gui', 'Trimnell', 'Drain', '5ºBFM O');
insert into patients (name, last_name, last_name2, course) values ('Stace', 'Lofting', 'Mudle', '0ºORD P');
insert into patients (name, last_name, last_name2, course) values ('Beverly', 'Churchlow', 'Kagan', '8ºFMY T');
insert into patients (name, last_name, last_name2, course) values ('Buck', 'Lonnon', 'Dallander', '3ºJVH E');
insert into patients (name, last_name, last_name2, course) values ('Berty', 'Leonardi', 'Goodbur', '7ºMKH D');
insert into patients (name, last_name, last_name2, course) values ('Sharlene', 'McKeighan', 'Wann', '5ºJPC M');
insert into patients (name, last_name, last_name2, course) values ('Linea', 'Fenge', 'Springtorpe', '7ºVKR O');
insert into patients (name, last_name, last_name2, course) values ('Garvey', 'Gallatly', 'Cawsby', '3ºXCD F');
insert into patients (name, last_name, last_name2, course) values ('Damian', 'Milella', 'Mutch', '4ºCVL I');
insert into patients (name, last_name, last_name2, course) values ('Adelice', 'Eamer', 'Cramond', '7ºTSS C');
insert into patients (name, last_name, last_name2, course) values ('Nance', 'Swinbourne', 'Wilce', '5ºRTR O');
insert into patients (name, last_name, last_name2, course) values ('Harland', 'Elner', 'Punyer', '9ºOLN S');
insert into patients (name, last_name, last_name2, course) values ('Cad', 'Leehane', 'Holttom', '0ºEKH S');
insert into patients (name, last_name, last_name2, course) values ('May', 'O''Henecan', 'Theseira', '8ºPRM N');
insert into patients (name, last_name, last_name2, course) values ('Idelle', 'Montier', 'Medgwick', '5ºDLS R');
insert into patients (name, last_name, last_name2, course) values ('Artemis', 'Heselwood', 'Jamrowicz', '4ºEVT H');
insert into patients (name, last_name, last_name2, course) values ('Dukie', 'Chance', 'Ellingsworth', '4ºKTX G');
insert into patients (name, last_name, last_name2, course) values ('Mic', 'Hulstrom', 'Rutter', '7ºKDJ I');
insert into patients (name, last_name, last_name2, course) values ('Florence', 'Moyers', 'Galbreath', '2ºMLM J');
insert into patients (name, last_name, last_name2, course) values ('Ginelle', 'Wisby', 'Gehrels', '4ºSBH X');
insert into patients (name, last_name, last_name2, course) values ('Stephi', 'Hodges', 'Rockell', '9ºTBI Z');
insert into patients (name, last_name, last_name2, course) values ('Kiri', 'Epps', 'Gheraldi', '7ºZFC X');
insert into patients (name, last_name, last_name2, course) values ('Jyoti', 'Silberschatz', 'Suddock', '6ºSYL Q');
insert into patients (name, last_name, last_name2, course) values ('Marwin', 'Colvie', 'MacAndrew', '0ºVPW K');
insert into patients (name, last_name, last_name2, course) values ('Domeniga', 'Gruczka', 'Lawland', '7ºAQP J');
insert into patients (name, last_name, last_name2, course) values ('Dian', 'Darke', 'Collcott', '3ºRME A');
insert into patients (name, last_name, last_name2, course) values ('Merrick', 'Doddemeade', 'Taborre', '2ºCGV G');
insert into patients (name, last_name, last_name2, course) values ('Berkly', 'Bercher', 'Cable', '4ºTJN W');
insert into patients (name, last_name, last_name2, course) values ('Emeline', 'Bodell', 'Penketh', '4ºALE W');
insert into patients (name, last_name, last_name2, course) values ('Skylar', 'Cansfield', 'Armatys', '2ºRAC Y');
insert into patients (name, last_name, last_name2, course) values ('Claiborn', 'Flood', 'Jarnell', '1ºFIP R');
insert into patients (name, last_name, last_name2, course) values ('Fan', 'Philp', 'Lukesch', '2ºKTP K');
insert into patients (name, last_name, last_name2, course) values ('Cesya', 'O''Hanley', 'Tither', '4ºMLD E');
insert into patients (name, last_name, last_name2, course) values ('Sallee', 'Liptrod', 'Abelwhite', '8ºWNI F');
insert into patients (name, last_name, last_name2, course) values ('Olympie', 'Cathrall', 'Batstone', '5ºHEF X');
insert into patients (name, last_name, last_name2, course) values ('Aron', 'Strick', 'Barrasse', '9ºBBK E');
insert into patients (name, last_name, last_name2, course) values ('Mario', 'Basindale', 'Colly', '0ºMIH Z');
insert into patients (name, last_name, last_name2, course) values ('Jarrett', 'Tibols', 'Pincott', '8ºMXY B');
insert into patients (name, last_name, last_name2, course) values ('Emili', 'Lait', 'Cecere', '1ºEZH N');
insert into patients (name, last_name, last_name2, course) values ('Belia', 'Hawse', 'Nodin', '4ºZVK R');
insert into patients (name, last_name, last_name2, course) values ('Hyacinthie', 'Barkway', 'Crolla', '7ºJEE D');
insert into patients (name, last_name, last_name2, course) values ('Alexandro', 'Quirke', 'Gurner', '2ºRLI F');
insert into patients (name, last_name, last_name2, course) values ('Beauregard', 'Ranken', 'Ziem', '3ºXSZ N');
insert into patients (name, last_name, last_name2, course) values ('Tiffi', 'O''Brogan', 'Halvorsen', '1ºLUZ M');
insert into patients (name, last_name, last_name2, course) values ('Magdalene', 'Bloy', 'Shimwall', '6ºQWC U');
insert into patients (name, last_name, last_name2, course) values ('Edmon', 'Hoggan', 'Frail', '0ºXKX D');
insert into patients (name, last_name, last_name2, course) values ('Sherm', 'Gelder', 'Sitch', '8ºDRL H');
insert into patients (name, last_name, last_name2, course) values ('Susannah', 'Sharram', 'Youster', '9ºWGH A');
insert into patients (name, last_name, last_name2, course) values ('Davin', 'Talton', 'Frankom', '5ºCPO I');
insert into patients (name, last_name, last_name2, course) values ('Brear', 'Maharg', 'Cowap', '8ºLNH X');
insert into patients (name, last_name, last_name2, course) values ('Nikolos', 'Coupland', 'Kildea', '9ºHIJ R');
insert into patients (name, last_name, last_name2, course) values ('Nikolaus', 'Heakins', 'Pilmoor', '5ºQSH Q');
insert into patients (name, last_name, last_name2, course) values ('Karisa', 'Lamond', 'Webbe', '4ºNNI X');
insert into patients (name, last_name, last_name2, course) values ('Emanuel', 'Irce', 'Josland', '3ºYHT L');
insert into patients (name, last_name, last_name2, course) values ('Chanda', 'Hunnicutt', 'Eliesco', '4ºCPV J');
insert into patients (name, last_name, last_name2, course) values ('Bren', 'Ebdin', 'Wilcher', '7ºLPC F');


