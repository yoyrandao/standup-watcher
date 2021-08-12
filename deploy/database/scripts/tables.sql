CREATE TABLE public.events (
	id SERIAL PRIMARY KEY,
	eventId VARCHAR(32) NOT NULL,
	data BYTEA NOT NULL,
	status INTEGER NOT NULL
);

CREATE TABLE public.notifications (
	id SERIAL PRIMARY KEY,
	data BYTEA NOT NULL,
	notificationSent BOOLEAN
);

CREATE TABLE public.subscribers (
	id SERIAL PRIMARY KEY,
	chatId BIGINT NOT NULL
);