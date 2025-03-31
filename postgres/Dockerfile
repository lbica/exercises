FROM postgres:latest

# Set environment variables for default username, password, and database
ENV POSTGRES_USER=airflow
ENV POSTGRES_PASSWORD=airflow
ENV POSTGRES_DB=dwh

# Optionally, copy custom configuration files
# For example, you can copy a custom `init.sql` file to initialize the database
COPY ./init_ddl.sql /docker-entrypoint-initdb.d/

# Expose the default PostgreSQL port
EXPOSE 5432

# Default command to run PostgreSQL server
CMD ["postgres"]