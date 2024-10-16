CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

insert into asp_net_roles(id, name, concurrency_stamp, normalized_name)
VALUES
(uuid_generate_v4()::text, 'Administrator', uuid_generate_v4()::text, 'ADMINISTRATOR'),
(uuid_generate_v4()::text, 'User', uuid_generate_v4()::text, 'USER');

insert into payment_types(id, name)
VALUES
(uuid_generate_v4(), 'Cash'),
(uuid_generate_v4(), 'Debit Card'),
(uuid_generate_v4(), 'Credit Card');

insert into currencies(id, name, abbreviation)
VALUES
(uuid_generate_v4(), 'Bulgarian lev', 'BGN'),
(uuid_generate_v4(), 'European Euro', 'EUR'),
(uuid_generate_v4(), 'U.S. Dollar', 'USD');



insert into categories(id, name, category_type, parent_category_id, is_initial)
values
(uuid_generate_v4(), 'Groceries', 0, null, true),
(uuid_generate_v4(), 'Kids', 0, null, true),
(uuid_generate_v4(), 'Housing', 0, null, true),
(uuid_generate_v4(), 'Car', 0, null, true),
(uuid_generate_v4(), 'Healh Care', 0, null, true),
(uuid_generate_v4(), 'Transportation', 0, null, true),
(uuid_generate_v4(), 'Financial Expenses', 0, null, true),

(uuid_generate_v4(), 'Eating out', 1, null, true),
(uuid_generate_v4(), 'Shopping', 1, null, true),
(uuid_generate_v4(), 'Life', 1, null, true),
(uuid_generate_v4(), 'Investments', 1, null, true),
(uuid_generate_v4(), 'Other', 1, null, true),

(uuid_generate_v4(), 'Income', 2, null, true),

(uuid_generate_v4(), 'Transfer', 3, null, true);

-- Add Housing sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Housing' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Housing' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Bills', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Mortgage', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Rent', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Furniture', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Maintenance, repairs', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Services', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Internet', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Phone, mobile phone', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'New house', parent_category_type, parent_category_id, true);
END $$;

-- Add Car sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Car' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Car' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Car Maintenance', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Fuel', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Parking', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Car Insurance', parent_category_type, parent_category_id, true);
END $$;

-- Add Healh Care sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Healh Care' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Healh Care' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Doctor', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Medicaments', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Dentist', parent_category_type, parent_category_id, true);
END $$;

-- Add Transportation sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Transportation' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Transportation' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Taxi', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Public Transport', parent_category_type, parent_category_id, true);
END $$;

-- Add Financial Expenses sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Financial Expenses' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Financial Expenses' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Taxes', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Charges & Fees', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Fines', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Bank fees', parent_category_type, parent_category_id, true);
END $$;

-- Add Eating out sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Eating out' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Eating out' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Fast Food', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Resturants', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Bar Cafe', parent_category_type, parent_category_id, true);
END $$;

-- Add Shopping sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Shopping' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Shopping' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Clothes', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Electronics', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Books', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Gifts', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Partner', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Pets', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Stationery, tools', parent_category_type, parent_category_id, true);
END $$;

-- Add Life sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Life' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Life' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Hobbies', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Vape', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Sports', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Education', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Online Services', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Alcohol, tobacco', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Holiday, trips, hotels', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Wellness, beauty', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Charity', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Cinema', parent_category_type, parent_category_id, true);
END $$;

-- Add Investments sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Investments' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Investments' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Savings Account', parent_category_type, parent_category_id, true);
END $$;

-- Add Other sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Other' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Other' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Missing', parent_category_type, parent_category_id, true);
END $$;

-- Add Income sub-categories
DO $$
DECLARE 
    parent_category_id uuid;
    parent_category_type integer;
BEGIN
    parent_category_id := (SELECT id from categories where name = 'Income' LIMIT 1);
    parent_category_type := (SELECT category_type from categories where name = 'Income' LIMIT 1);
    
    insert into categories(id, name, category_type, parent_category_id, is_initial)
    values
    (uuid_generate_v4(), 'Salary', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Bank Loan', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Refunds (tax, purchase)', parent_category_type, parent_category_id, true),
    (uuid_generate_v4(), 'Interests, dividends', parent_category_type, parent_category_id, true);
END $$;

