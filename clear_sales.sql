-- Скрипт для очистки всех продаж из базы данных
-- ВНИМАНИЕ: Это удалит все записи из таблицы Sales
-- Товары (Products) с SaleId будут иметь SaleId = null после этого

-- Очистка таблицы продаж
TRUNCATE TABLE "Sales" RESTART IDENTITY CASCADE;

-- Сброс SaleId для всех товаров
UPDATE "Products" SET "SaleId" = NULL WHERE "SaleId" IS NOT NULL;
