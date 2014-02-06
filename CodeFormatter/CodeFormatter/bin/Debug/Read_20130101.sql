dsdsadasdadasd_daasda:
DECLARE @StreamDetailID	   int = 681;						  -- The ID of the stream which should be loaded into CFGETL schema
DECLARE @ModuleDetailID	   int = 681;						  -- The ID of current stream module 
DECLARE @boolOnlyRowsCont  nvarchar( 1 ) = N'0';		  -- A value controls truncate and validation procedures																			
DECLARE @boolRunArchive	   nvarchar( 1 ) = N'0';		  -- A value controls if archivation will used
DECLARE @boolRunVerifySP   nvarchar( 1 ) = N'0';		  -- A value controls validation procedure run
DECLARE @DataFileDir		   nvarchar( 1024 ) = N'C:\4work\CorpIT-FSSTax\Audit DA\SSIS\SAP Data Files load\data-sap';  -- Contains path to the data directory
DECLARE @ErrorDir			   nvarchar( 1024 ) = N'C:\4work\CorpIT-FSSTax\Audit DA\SSIS\SAP Data Files load\error-sap'; -- Contaians path to the directory with error logs
DECLARE @loadName			   nvarchar( 1024 ) = N'MISC_SAPAGR_WeeklyLoad';      -- Contains package name 
DECLARE @resultMailTo	   nvarchar( 1024 ) = N'vvcx@chevron.com';				-- Holds a receiver’s mail address
DECLARE @SAPfileNameMask   nvarchar( 1024 ) = N'PC*_AGR_USERS.TXT';				-- SAP Flat file search mask

-- Holds connection string for AuditDA connection manager
DECLARE @auditDAconnection nvarchar( 1024 ) = N'Data Source=.;Initial Catalog=AuditDAD;Provider=SQLNCLI10.1;Integrated Security=SSPI;Application Name=SSIS-ADR_US_WeeklyLoad-{6C1DE0EA-3CE1-4125-88F7-2E28F4729E13}LocalHost.AuditDAD;Auto Translate=False;';

ON

/*** STATIC VARIABLES ***/
/*** ScriptSystemVariables ***/
DECLARE @boolServerSwitch nvarchar( 4 ) = N'Test';		  --Test or Prod
DECLARE @DBname			  nvarchar( 64 ) = N'AuditDA';  --Holds datadase name
DECLARE @serverName		  nvarchar( 2 );					  --Contains server name('43' or '91')
DECLARE tableNames		  CURSOR
     FOR SELECT 'CFGETL.StreamDetail' AS Table_name  
         UNION
         SELECT 'CFGETL.StreamAttribute'
         UNION
         SELECT 'CFGETL.ModuleDetail'
         UNION
         SELECT 'CFGETL.ModuleAttribute'
         UNION
         SELECT 'CFGETL.StreamModuleDetail'
         ORDER BY 1;												  --The cursor with the tables for inserting

/*** ModuleDetailVariables ***/
DECLARE @packageName nvarchar( 100 ) = N'';


/*** StreamDetail Variables ***/
DECLARE @StreamTypeID			smallint = 2;
DECLARE @EnabledFlag				bit = 1;
DECLARE @RestartSupportedFlag bit = 1;
DECLARE @CleanUpType				tinyint = 2;
DECLARE @CleanUpInterval		smallint = 30;
DECLARE @debug_counter			int = 0;

/*** StreamModuleDetail Variables ***/
DECLARE @Order									int = 1;
DECLARE @StreamModuleDetail_EnableFlag bit = 1;
DECLARE @CompleteFlag						bit = 0;

/*** AdditionalVariables ***/
DECLARE @msg				  nvarchar( 128 ) = 'There are concurent records in table ';
DECLARE @cnt				  int = 0;
DECLARE @curTable			  nvarchar( 128 );

/*** sp_executesql procedure variables ***/
DECLARE @StreamDefinition nvarchar( 128 );
DECLARE @ModuleDefinition nvarchar( 128 );
DECLARE @StreamSQL		  nvarchar( 512 );
DECLARE @ModuleSQL		  nvarchar( 412 );
DECLARE @result			  int;



IF @boolServerSwitch = N'Test'										-- Switch between production and non-production server
BEGIN
    SET @serverName = N'43';
    USE AuditDAD;
END;

IF @boolServerSwitch = N'Prod'
BEGIN
    SET @serverName = N'91';
    USE AuditDAP;
END;

OPEN tableNames;															-- Open cursor
FETCH FROM tableNames INTO @curTable;								-- Start fetching table names into @curable variable

WHILE @@FETCH_STATUS = 0
BEGIN
    FETCH FROM tableNames INTO @curTable;							-- Fetch next table name
    
    /*** Stream tables checking block ***/
    IF @curTable = 'CFGETL.StreamDetail'						
    OR @curTable = 'CFGETL.StreamAttribute'
    OR @curTable = 'CFGETL.StreamModuleDetail'
    BEGIN
    
        SET @StreamSQL = N'SELECT @rezul = (SELECT COUNT(*) FROM ' + @curTable + ' where StreamDetailID = @StreamDetailIDTemp)';
        SET @StreamDefinition = N' @rezul int OUTPUT, @StreamDetailIDTemp int';
        
        EXECUTE sp_executesql @StreamSQL, @StreamDefinition, @StreamDetailIDTemp = @StreamDetailID, @rezul = @result OUTPUT; -- checking if some exists 
        IF @result <> 0													-- If row count with current @StreamDetailID not equal to 0	
        BEGIN
            SELECT @msg + @curTable;								-- Generating the message with error table
            SELECT @cnt = 1;											-- @cnt controls if transaction will start (if 0 - Yes, 1 - No)
        END;														
	 END;
	 
	 /*** Module tables checking block***/
        ELSE															
        BEGIN
        
            SET @ModuleSQL = N'SELECT @rezul = (SELECT COUNT(*) FROM ' + @curTable + ' where ModuleDetailID = @ModuleDetailIDTemp)';
            SET @ModuleDefinition = N' @ModuleDetailIDTemp int, @rezul int OUTPUT';
            
            EXECUTE sp_executesql @ModuleSQL, @ModuleDefinition, @ModuleDetailIDTemp = @ModuleDetailID, @rezul = @result OUTPUT; -- checking if some exists
            IF @result <> 0											-- If row count with current ModuleDetailID not equal to 0			 
            BEGIN
					SELECT @msg + @curTable;							-- Generating the message with error table
					SELECT @cnt = 1;										-- @cnt controls if transaction will start (if 0 - Yes, 1 - No)
            END;
        END;
        
END;

CLOSE tableNames;															-- Close cursor
DEALLOCATE tableNames;

BEGIN
    BEGIN TRANSACTION;
    
    IF @cnt = 0
    BEGIN
	
        /*** Filling StreamDetail table ***/
        INSERT INTO CFGETL.StreamDetail( StreamDetailID
                                       , StreamDetailDesc
                                       , StreamTypeID
                                       , EnabledFlag
                                       , RestartSupportedFlag
                                       , CleanUpType
                                       , CleanUpInterval )
        VALUES( @StreamDetailID, @loadName + N' package', @StreamTypeID, @EnabledFlag, @RestartSupportedFlag, @CleanUpType, @CleanUpInterval );

        /*** Filling StreamAttribute table ***/
        INSERT INTO CFGETL.StreamAttribute( StreamDetailID
                                          , AttributeName
                                          , AttributeValue
                                          , DataType
                                          , LastUpdatedDate )
        VALUES( @StreamDetailID, N'auditDAconnection', @auditDAconnection, N'String', GETDATE( ))
            , ( @StreamDetailID, N'boolOnlyRowsCont', @boolOnlyRowsCont, N'String', GETDATE( ))
            , ( @StreamDetailID, N'boolRunArchive', @boolRunArchive, N'String', GETDATE( ))
            , ( @StreamDetailID, N'boolRunVerifySP', @boolRunVerifySP, N'String', GETDATE( ))
            , ( @StreamDetailID, N'DataFileDir', @DataFileDir, N'String', GETDATE( ))
            , ( @StreamDetailID, N'DataFileDir_' + @serverName, @DataFileDir + '_' + @serverName, N'String', GETDATE( ))
            , ( @StreamDetailID, N'ErrorDir', @ErrorDir, N'String', GETDATE( ))
            , ( @StreamDetailID, N'loadName', @loadName, N'String', GETDATE( ))
            , ( @StreamDetailID, N'resultMailTo', @resultMailTo, N'String', GETDATE( ))
            , ( @StreamDetailID, N'SAPfileNameMask', @SAPfileNameMask, N'String', GETDATE( ));


        /*** Filling ModuleDetail table ***/
        INSERT INTO CFGETL.ModuleDetail( ModuleDetailID
                                       , ModuleDetailDesc
                                       , ModulePackageName )
        VALUES( @ModuleDetailID, @loadName + N' package', @packageName + N'.dtsx' );

        /*** Filling ModuleAttribute table ***/
/*
        INSERT INTO CFGETL.ModuleAttribute( ModuleDetailID
                                          , AttributeName
                                          , AttributeValue
                                          , DataType
                                          , LastUpdatedDate )
        VALUES( @ModuleDetailID, N'auditDAconnection', @auditDAconnection, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'boolOnlyRowsCont', @boolOnlyRowsCont, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'boolRunArchive', @boolRunArchive, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'boolRunVerifySP', @boolRunVerifySP, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'DataFileDir', @DataFileDir, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'DataFileDir_' + @serverName, @DataFileDir + '_' + @serverName, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'ErrorDir', @ErrorDir, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'loadName', @loadName, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'resultMailTo', @resultMailTo, N'String', GETDATE( ))
            , ( @ModuleDetailID, N'SAPfileNameMask', @SAPfileNameMask, N'String', GETDATE( ));
*/				   

			   
        /*** Filling StreamModuleDetail table ***/
        INSERT INTO CFGETL.StreamModuleDetail( StreamModuleDetailID
                                             , StreamDetailID
                                             , ModuleDetailID
                                             , [Order]
                                             , EnabledFlag
                                             , CompleteFlag )
        VALUES( @StreamDetailID, @StreamDetailID, @ModuleDetailID, @Order, @StreamModuleDetail_EnableFlag, @CompleteFlag );

    COMMIT TRANSACTION; 
    END;
END;

	

	
	
	
