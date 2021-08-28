CREATE TABLE public.events (
	id SERIAL PRIMARY KEY,
	eventId VARCHAR(32) NOT NULL,
	data BYTEA NOT NULL,
	status INTEGER NOT NULL,
	creationTimestamp TIMESTAMP NOT NULL,
	modificationTimestamp TIMESTAMP NOT NULL
);

CREATE TABLE public.notifications (
	id SERIAL PRIMARY KEY,
	data BYTEA NOT NULL,
	notificationSent BOOLEAN,
	creationTimestamp TIMESTAMP NOT NULL,
	modificationTimestamp TIMESTAMP NOT NULL
);

CREATE TABLE public.subscribers (
	id SERIAL PRIMARY KEY,
	chatId BIGINT NOT NULL,
	username VARCHAR(255) NOT NULL,
	creationTimestamp TIMESTAMP NOT NULL,
	modificationTimestamp TIMESTAMP NOT NULL
);

ALTER TABLE public.events ALTER COLUMN creationTimestamp SET DEFAULT NOW();
ALTER TABLE public.events ALTER COLUMN modificationTimestamp SET DEFAULT NOW();

ALTER TABLE public.subscribers ALTER COLUMN creationTimestamp SET DEFAULT NOW();
ALTER TABLE public.subscribers ALTER COLUMN modificationTimestamp SET DEFAULT NOW();

ALTER TABLE public.notifications ALTER COLUMN creationTimestamp SET DEFAULT NOW();
ALTER TABLE public.notifications ALTER COLUMN modificationTimestamp SET DEFAULT NOW();