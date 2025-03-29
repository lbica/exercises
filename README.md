# BIS - Coding Exercise Data Analytics Engineer

## Installation

### Docker

Please install Docker on Linux or on Windows use Docker Desktop. Open a command prompt and navigate to the following path:
bis-coding-exercise/airflow and run the following command:

*docker-compose up -d*

After the execution all services must run in docker:

![Docker Compose](/assets/img/2.png)

*docker build -f webapi-service/Dockerfile -t webapi-image .*

and then *docker run -p 5163:5163 --name webapi-service webapi-image* to run the container

It will create a image using the Dockerfile from web-service folder that can be used in *docker-compose.yml*

## Services Description

### MySqlServer description

This interface provides ERP P10 Cost Center data for Salesforce Countries from DES into GCRM Salesforce as a new object. Cost Centers are required to allocate costs for creating sample orders. Stakeholders are sales and marketing users which are allowed to create sample orders. Contacts during implementation were Thomas Scharfenberger from DPS Sales and Trutz Pohlenz from Business Area perspective.

### Airflow description

Please runs the follwing commands in order to start Airflow services.

Relevant connection properties:


| Name         | Type   | Value                                                                                                                                 |
| -------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| `user`       | config | airflow                                                                                                                               |
| `pass`       | config | airflow                                                                                                                               |
| `web-ui-url` | config | [http://localhost:8080/](http://localhost:8080/)                                                                                      |
|              |        | ](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-connections/-/blob/master/prod/config/salesforce_gcrm.prod) |

### PostGres

In order to conect to Postgres and execute the sql stament please use the follwing steps:

1. Run from command prompt *docker exec -it airflow-postgres-1 /bin/bash*
2. One you are inside tje containte execute: *psql -h localhost -U airflow -d airflow*
3. the psql cli will be shown

![PSQL](/assets/img/1.png)

### WebApiServer description

This is a web api service that will handle CRUD operations send by clients on port 5000.

### Swagger URL

http://localhost:5000/swagger/index.html

## Job Design

#### I705_gcrm_salesforce_costcenter_main 0.1

The flowchart below shows process implementation

![Process Description](/img/interfaces/customer-relationship-management/salesforce/ti705/ti705_costcenter_process_diagram.svg)

### Connection Properties

Relevant connection properties:


| Name                       | Type       | Link                                                                                                                                                                                                   |
| ---------------------------- | ------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `connection.properties`    | Config     | [config/prod/gcrm/projectConfig/connection.properties)](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-v3-config/-/blob/master/prod/gcrm/projectConfig/connection.properties) |
| `talendManage.prod`        | Connection | [connections/prod/config/talendManage.prod](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-connections/-/blob/develop/prod/config/talendManage.prod)                          |
| `des_dataintegration.prod` | Connection | [connections/prod/config/des_dataintegration.prod](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-connections/-/blob/develop/prod/config/des_dataintegration.prod)            |
| `salesforce.prod`          | Connection | [connections/prod/config/salesforce_gcrm.prod](https://git-go.europe.phoenixcontact.com/ints-talend/pxc-cimt-framework-connections/-/blob/master/prod/config/salesforce_gcrm.prod)                     |

<br/>

### Job specific Talend context properties


| Parameter                  | Example                                                                                             | Description/Comment                                                                |
| ---------------------------- | ----------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| p_force_load_from_date_utc | 1970-01-01 00:00:00                                                                                 | Start timestamp for full or partial reload. Leave empty for normal delta operation |
| p_filter_vkorg             | "'1040','3140', '3160','3070','3040','3200','3010','3130','3060',<br />'3080','3100','1010','1015'" | This filter will select only this list of organization units                       |

<br/>

### Processing Steps / Mapping Rules

<br/>

**Process Step 1**


| Step             | Description                                                                                                  | Business Entity              | Business Rule | Failure/Support |
| ------------------ | -------------------------------------------------------------------------------------------------------------- | ------------------------------ | --------------- | ----------------- |
| 1. Read DES data | Read data from DI_STAGE.SAPP10100_0COSTCENTER_ATTR_CURRENT,<br />DI_STAGE.SAPP10100_0COSTCENTER_TEXT_CURRENT | COSTCENTER related documents | see below     | -               |

**Business Rules**


| # | Title                                                        | Description                                                                                                         |
| --- | -------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| 1 | Select active CostCenter                                     | SAPP10100_0COSTCENTER_ATTR_CURRENT.DATETO = '2099-12-31' (means null in SAP). Only active cost centers are uploaded |
| 2 | Select not blocked CostCenter                                | SAPP10100_0COSTCENTER_ATTR_CURRENT.BKZKP is null. x means cost center is blocked                                    |
| 3 | Cost center with BKZR = 'X' can´t be used for sample orders | SAPP10100_0COSTCENTER_ATTR_CURRENT.BKZER is null                                                                    |
|   |                                                              |                                                                                                                     |

<br/>

**Process Step 2**


| Step                                                                  | Description | Business Entity | Business Rule | Failure/Support |
| ----------------------------------------------------------------------- | ------------- | ----------------- | --------------- | ----------------- |
| 1.Insert data into I705_GCRM_SALESFORCE_COSTCENTER_SEND staging table | type        | `CostCenter`    | see below     | -               |

**Business Rules**


| # | Title                                                                   | Description                                                                                                         |
| --- | ------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| 1 | retrieve only delta changes and SalesOrg based on p_filter_vkorg values | META_CORE_ENDDATE > (String)globalMap.get("last_run_date_as_sql_utc") AND  SALESORG__C IN ( context.p_filter_vkorg) |
| 2 | generate new files                                                      | sf_CostCenter__c_insert0.csv, sf_CostCenter__c_insert1.csv etc                                                      |

<br/>
<br/>

**Process Step 3**


| Step                               | Description | Business Entity | Business Rule | Failure/Support |
| ------------------------------------ | ------------- | ----------------- | --------------- | ----------------- |
| 1.Upload csv files into SalesForce | type        | `CostCenter__c` | see below     | -               |

**Business Rules**


| # | Title                                                                              | Description                                                                                                                                                                                                                                 |
| --- | ------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1 | For each file send page_size records to SalesForce using bulk api upsert operation | For each file send page_size records to SalesForce using bulk api upsert operationselect ASSET_ID   from DWL_ASSETS   WHERE SOURCE = 'teamcenter' AND TYPE = 'cad_pic' OR TYPE = 'tech_drawing' AND FILE_NAME =\<generated_target_file_name |
| 2 | If failed catch the error into session table                                       | UPDATE I705_gcrm_salesforce_sessions with failure message and error status FAILED                                                                                                                                                           |
| 3 | If success update with success status into session table                           | UPDATE I705_gcrm_salesforce_sessions with success status                                                                                                                                                                                    |

**Mapping**


| Target field            | Source field / Transformation rule           |
| ------------------------- | ---------------------------------------------- |
| CostCenterID__c         | SAPP10100_0COSTCENTER_ATTR_CURRENT.OBJNR     |
| CostCenterNumber__c     | SAPP10100_0COSTCENTER_ATTR_CURRENT.KOSTL     |
| FunctionalAreaNumber__c | SAPP10100_0COSTCENTER_ATTR_CURRENT.FUNC_AREA |
| SalesOrg__c             | SAPP10100_0COSTCENTER_ATTR_CURRENT.BUKRS     |
| Name                    | SAPP10100_0COSTCENTER_TEXT_CURRENT.TXTMD     |
|                         |                                              |

Release Notes


| Jira                                                                                                                                                          | Publish Version | Developer           |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------- | --------------------- |
| [[DMI-9074] GCRM: Upload of cost center - JIRA](https://jira.europe.phoenixcontact.com/browse/DMI-9074)https://jira.europe.phoenixcontact.com/browse/DMI-688) | 1.0.0           | Laurentiu<br />Bica |
|                                                                                                                                                               |                 |                     |

## Operation


| Task Name                            | Project |
| -------------------------------------- | --------- |
| I705_gcrm_salesforce_costcenter_main | gcrm    |

### Monitoring

Monitoring is done with the Operational Dashboard and TAC in our daily standups

### Job Trigger

CronTrigger daily at 6 am ,14 pm and 20pm

### Error Handling

All relevant Talend components have option "Die on error" activated, so if any processing step fails, the entire job run will also fail. Restart will reprocess the last delta again (delta determination by tJobInstanceStart_1_PREV_JOB_START_DATE).
