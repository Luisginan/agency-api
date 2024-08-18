-- Blueprint/Table.sql
-- This file contains the SQL schema definitions for the Blueprint application.
-- It includes the creation of tables such as 'customer' and 'messaging_log'.
-- Each table is defined with its respective columns, data types, and constraints.

CREATE TABLE public.customer (
	id serial4 NOT NULL,
	address varchar NULL,
	email varchar NULL,
	phone varchar NULL,
	"name" varchar NULL,
    is_active bit NOT NULL,
	CONSTRAINT customer_pk PRIMARY KEY (id)
);

CREATE TABLE public.agency
(
    id serial4 NOT NULL,
    name varchar,
    address varchar,
    city varchar,
    CONSTRAINT agency_pk PRIMARY KEY (id)
);

CREATE TABLE public.appointment
(
    id serial4 NOT NULL,
    date date NOT NULL,
    description varchar NOT NULL DEFAULT '',
    agency_id int4 NOT NULL,
    customer_id int4 NOT NULL,
    CONSTRAINT appointment_pk PRIMARY KEY (id)
);

CREATE TABLE public.token_issuance
(
    id serial4 NOT NULL,
    token varchar NOT NULL DEFAULT '',
    issuance_date date NOT NULL,
    expiry_date date NOT NULL,
    customer_id int4 NOT NULL,
    agency_id int4 NOT NULL,
    appointment_id int4 NOT NULL,
    CONSTRAINT token_issuance_pk PRIMARY KEY (id)
);

CREATE TABLE public.agency_holiday
(
    id serial4 NOT NULL,
    agency_id int4 NOT NULL,
    holiday DATE NOT NULL,
    CONSTRAINT agency_holiday_pk PRIMARY KEY (id)

);



INSERT INTO public.agency (id, name, address, city) VALUES (DEFAULT, 'PT. Lumrah Sejati', 'Jakarta', 'Jakarta');
INSERT INTO public.agency_settings (id, agency_id, max_appointments_per_day) VALUES (DEFAULT, 1, 0);
INSERT INTO public.agency_holiday (id, agency_id, holiday) VALUES (DEFAULT, 1, '2021-12-25');
INSERT INTO public.customer (id, address, email, phone, name, is_active) VALUES (DEFAULT, 'Jakarta', 'john@test.com', '08723234444', 'John C', 'true');
INSERT INTO public.appointment (id, date, description, agency_id, customer_id) VALUES (DEFAULT, '2024-08-17', 'Meeting', 1, 1);





