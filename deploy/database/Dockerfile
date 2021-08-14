FROM postgres:alpine

ADD scripts/1_createdb.sql /docker-entrypoint-initdb.d
ADD scripts/2_init.sql /docker-entrypoint-initdb.d

RUN chmod a+r /docker-entrypoint-initdb.d/*
EXPOSE 5432