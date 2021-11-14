CREATE SCHEMA IF NOT EXISTS `MinesweeperApp` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `MinesweeperApp` ;

-- -----------------------------------------------------
-- Table `MinesweeperApp`.`users`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `MinesweeperApp`.`users` ;

CREATE TABLE IF NOT EXISTS `MinesweeperApp`.`users` (
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