-- MySQL Script generated by MySQL Workbench
-- Sun Dec 12 13:12:36 2021
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema cst-350
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema cst-350
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `cst-350` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `cst-350` ;

-- -----------------------------------------------------
-- Table `cst-350`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `cst-350`.`users` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `FIRSTNAME` VARCHAR(40) NOT NULL,
  `LASTNAME` VARCHAR(40) NOT NULL,
  `USERNAME` VARCHAR(40) NOT NULL,
  `EMAIL` VARCHAR(40) NOT NULL,
  `SEX` VARCHAR(10) NOT NULL,
  `AGE` INT NOT NULL,
  `STATE` VARCHAR(10) NOT NULL,
  `PASSWORD` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `ID_UNIQUE` (`ID` ASC) VISIBLE,
  UNIQUE INDEX `USERNAME_UNIQUE` (`USERNAME` ASC) VISIBLE,
  UNIQUE INDEX `EMAIL_UNIQUE` (`EMAIL` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `cst-350`.`boards`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `cst-350`.`boards` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `USER_ID` INT NOT NULL,
  `SIZE` INT NOT NULL,
  `DIFFICULTY` INT NOT NULL,
  `NUMBEROFMINES` INT NOT NULL,
  `GRID` VARCHAR(2400) CHARACTER SET 'utf8' NOT NULL,
  `TIMESTARTED` DATETIME NOT NULL,
  `TIMEPLAYED` TIME NOT NULL,
  PRIMARY KEY (`ID`),
  INDEX `FK_BOARDS_TO_USERS_idx` (`USER_ID` ASC) VISIBLE,
  CONSTRAINT `FK_BOARDS_TO_USERS`
    FOREIGN KEY (`USER_ID`)
    REFERENCES `cst-350`.`users` (`ID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
