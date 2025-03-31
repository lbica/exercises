# BIS - Coding Exercise Data Analytics Engineer

## Installation Requirements

### Docker

Please install Docker on Linux or on Windows use Docker Desktop. For this version Docker Desktop has been used on Windows 11.

### GitHub

Please install git client on your local machine or go to github [https://github.com/lbica/exercises](https://github.com/lbica/exercises) and download the source code. For clone use the follwing command in command prompt:

*git clone https://github.com/lbica/exercises*

For building the solution I have used Visual Studio Code and Visual Studio 2022 for webapi-service. However if you can open the root folder on Visual Studio Code will works too.

## Solution/Project Structure

The solution consist of many folders that are built for difference goal. In the folowing section I will present in a nuschel the goal of directory.

![Visual Studio Code Project Structure](/assets/img/4.png)

### etl-service description

This service is the **ETL solution** for reading the input files (customers, products and order), apply the cleansing rules and load the currated data into a dimensional  model. However, in a real project the ingestion, cleansing/validation and results error including metadata  should be stored using a **Kimbal** approach with staging, core and mart layers or using a **Medalion** style: bronze, silver and gold layers.

The etl-service consists of etl_task.py script that implmented many types of validation classified as following:


| Name               | Type            | Description                                                                                                     |
| -------------------- | ----------------- | ----------------------------------------------------------------------------------------------------------------- |
| `empty_column`     | validation rule | This rule eliminates the empty value of a given column and logs the errors in log file for keep trace and debug |
|                    |                 |                                                                                                                 |
| `invalid_datatype` | validation rule |                                                                                                                 |
| `negative_value`   | validation rule |                                                                                                                 |

The folder *data* consist of input data received by ftp or other upstream processes. This is the input for **etl_task.py** script. For each validation rule there is a specific method e.g
invalid_datatype that handle the rule based on a input dataframe and column name. Please check the comments inside each method for more details.

![Data folder](/assets/img/5.png)


![Data folder](/assets/img/6.png)

### Airflow description

In airflow folder I have created all requird configuration and dag script to orchestrate our etl-service. If you want to see the current scheduled task (according with specification) you can access the airflow UI using the following connection properties. For more details about how to start the services in docker container please check the Docker Configuration section.


| Name         | Type   | Value                                                                                                                                 |
| -------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| `user`       | config | airflow                                                                                                                               |
| `pass`       | config | airflow                                                                                                                               |
| `web-ui-url` | config | [http://localhost:8080/](http://localhost:8080/)                                                                                      |
|              |        | ](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-connections/-/blob/master/prod/config/salesforce_gcrm.prod) |

### webapi-service description

This service is built to handle the CRUD operations requests from users: postman, bruno or othe consumers. The application has been built using the ASP.Net Web API and dotnet8 as SDK.
The application is runnign in a docker container and can be accssed at this url: [http://localhost:5163/swagger](http://localhost:5163/swagger). You can use swagger to test any CRUD operation. Please note that webapi-service it's using PostGres as data storage.

![Swagger WebApi](/assets/img/10.png)

**Note**: For handling exception I have created a GlobalExceptionErrorMiddleware and ResponseExcpetionFilter than handles all exceptions and present the result to the consumer in a JSON format. Please check below the payload of error json outcome. Entity Framework has been used as ORM Framework and DTOs objects for request/reponse data transfer. Automapper for mapping between DTos and Entities and viceversa.



## Docker Containers

According with specification the entire solution using docker for containarization. Each folder (project) has it's own Dockerfile used t built the image for a dedicated purpose: e.g etl-service,
webapi-service etc

### docker-compose.yml

In order thet all services to comuunicate one each other I have used a docker-compose file (see in the root of the solution). This file consist of all services definition , enironment variable or other commands required for the appropitae service.  For running the docker-compose.yml file please check the below section

### Dockerfile

This interface provides ERP P10 Cost Center data for Salesforce Countries from DES into GCRM Salesforce as a new object. Cost Centers are required to allocate costs for creating sample orders. Stakeholders are sales and marketing users which are allowed to create sample orders. Contacts during implementation were Thomas Scharfenberger from DPS Sales and Trutz Pohlenz from Business Area perspective.

### Running the docker-compose.yml

Please follow the next steps in order to have all services up and running.

*Important*: Please check the Installation section before running. Docker must runs on the host machine.

1. open command prompt and go to the root application:


2. runs **docker-compose up -d**

after 1 min (please note that maybe will takes more time due the images must be downloaded from remote docker hub )

![Error Json](/assets/img/3.png)

bis-coding-exercise/airflow and run the following command:
*docker-compose up -d*

After the execution all services must run in docker:

![Docker Compose](/assets/img/2.png)


3. Please open the Docker Desktop or run docker ps. You should see all contaires in running state



4. Open a browser and navigate to [http://localhost:8080](https://localhost:8080). You should see now the Airflow Web Interface. Navigates to DAG and enable the cleasing and ingestion task for one of the entity. This will runs according with requiremetsL daily or hourly (for orders).

5. Go inside postgres docker comtainer and check the number of records for customers. You shoud  and empty table

6. Go back in browser enable the cleansing and ingestion task for one of the entity (e.g. customers). This will runs according with requiremetsL daily or hourly (for orders) and aftet few seconds shoudl runs and apply cleansing and ingest the data into postgres customer table

7. Go again inside postgres docker comtainer and check the number of records again for customers. You shoud  see now all currated records there.

### PostGres or SQLLite

In order to conect to Postgres and execute the sql stament please use the follwing steps:

1. Run from command prompt *docker exec -it airflow-postgres-1 /bin/bash*
2. One you are inside the container execute: *psql -h localhost -U airflow -d airflow*
3. the psql cli will be shown

![PSQL](/assets/img/1.png)


Use the **analytics.sql** file to run analytics statement on SqlLite or Postgres. For SqlLie I have used DB Browser for SqlLite.  

![SQlLite](/assets/img/8.png)


![SqlLite execution](/assets/img/9.png)


### Assets folder

This folders consists of Dimensional Model image and other images used inside **Readme.md** file.


![Dimensional Model](/assets/img/dimensional_model.png)


## Dimensional Modeling

- [Database Type](#database-type)
- [Table Structure](#table-structure)
  - [dim_products](#dim_products)
  - [dim_customers](#dim_customers)
  - [dim_date](#dim_date)
  - [fact_orders](#fact_orders)
- [Relationships](#relationships)
- [Database Diagram](#database-Diagram)

### Database type

- **Database system:** SQLLite

### Table structure

#### dim_products


| Name            | Type         | Settings                                | References | Note         |
| ----------------- | -------------- | ----------------------------------------- | ------------ | -------------- |
| **product_key** | INTEGER      | 🔑 PK, not null , unique, autoincrement |            |              |
| **stock_code**  | VARCHAR(30)  | not null , unique                       |            | Business key |
| **description** | VARCHAR(255) | not null                                |            |              |
| **unit_price**  | NUMERIC      | not null                                |            |              |

#### dim_customers


| Name             | Type         | Settings                                | References | Note         |
| ------------------ | -------------- | ----------------------------------------- | ------------ | -------------- |
| **customer_key** | INTEGER      | 🔑 PK, not null , unique, autoincrement |            |              |
| **customer_id**  | VARCHAR(30)  | not null , unique                       |            | Business key |
| **country**      | VARCHAR(255) | not null                                |            |              |

#### dim_date


| Name          | Type         | Settings                 | References | Note                              |
| --------------- | -------------- | -------------------------- | ------------ | ----------------------------------- |
| **date_key**  | INTEGER      | 🔑 PK, not null , unique |            | Format: YYYYMMDD (e.g., 20250330) |
| **full_date** | DATE         | not null , unique        |            | Business key                      |
| **year**      | INTEGER      | not null                 |            |                                   |
| **month**     | INTEGER      | not null                 |            |                                   |
| **day**       | INTEGER      | not null                 |            |                                   |
| **week_day**  | VARCHAR(255) | not null                 |            |                                   |

#### fact_orders


| Name             | Type         | Settings                      | References                                | Note               |
| ------------------ | -------------- | ------------------------------- | ------------------------------------------- | -------------------- |
| **invoice_no**   | VARCHAR(255) | 🔑 PK, not null               |                                           |                    |
| **invoice_date** | INTEGER      | 🔑 PK, not null               | fk_fact_orders_invoice_date_dim_date      | stored as YYYYMMDD |
| **product_key**  | INTEGER      | 🔑 PK, not null , default: -1 | fk_fact_orders_product_key_dim_products   |                    |
| **quantity**     | INTEGER      | not null                      |                                           |                    |
| **customer_key** | INTEGER      | 🔑 PK, not null , default: -1 | fk_fact_orders_customer_key_dim_customers |                    |

### Relationships

- **fact_orders to dim_products**: many_to_one
- **fact_orders to dim_customers**: many_to_one
- **fact_orders to dim_date**: many_to_one

### Database Diagram

```mermaid
erDiagram
	fact_orders }o--|| dim_products : references
	fact_orders }o--|| dim_customers : references
	fact_orders }o--|| dim_date : references

	dim_products {
		INTEGER product_key
		VARCHAR(30) stock_code
		VARCHAR(255) description
		NUMERIC unit_price
	}

	dim_customers {
		INTEGER customer_key
		VARCHAR(30) customer_id
		VARCHAR(255) country
	}

	dim_date {
		INTEGER date_key
		DATE full_date
		INTEGER year
		INTEGER month
		INTEGER day
		VARCHAR(255) week_day
	}

	fact_orders {
		VARCHAR(255) invoice_no
		INTEGER invoice_date
		INTEGER product_key
		INTEGER quantity
		INTEGER customer_key
	}
```
