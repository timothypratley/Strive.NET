BEGIN TRANSACTION

DECLARE @TerrainPieceSize INT
SET @TerrainPieceSize = 10
DECLARE @ForestTreeFrequency INT
SET @ForestTreeFrequency = 2
DECLARE @ForestTerrainTemplateLowest INT
SET @ForestTerrainTemplateLowest = 65814
DECLARE @ForestTerrainTemplateHighest INT
SET @ForestTerrainTemplateHighest = 65816

-- Script designed to be run again and again
DELETE FROM ObjectInstance 
WHERE TemplateObjectID IN (65814,65815,65816)

SELECT CAST(ROUND((RAND() * (@ForestTerrainTemplateHighest - @ForestTerrainTemplateLowest)) + @ForestTerrainTemplateLowest, 0) AS INT)

SELECT (RAND() * 10)

INSERT INTO ObjectInstance
SELECT CAST(ROUND((RAND(CAST(NEWID() AS varbinary(128))) * (@ForestTerrainTemplateHighest - @ForestTerrainTemplateLowest)) + @ForestTerrainTemplateLowest, 0) AS INT) AS TemplateObjectID, 
	ObjectInstance.X + (RAND(CAST(NEWID() AS varbinary(128))) *@TerrainPieceSize) AS X, 
	ObjectInstance.Y AS Y,
	ObjectInstance.Z + (RAND(CAST(NEWID() AS varbinary(128))) *@TerrainPieceSize) AS Z,
	0 AS RotationX,
	0 AS RotationY,
	0 AS RotationZ,
	0, 
	0
FROM ObjectInstance
	INNER JOIN TemplateTerrain
		ON ObjectInstance.TemplateObjectID = TemplateTerrain.TemplateObjectID
WHERE EnumTerrainTypeID = 9 AND
	CAST(X AS INT) % (@TerrainPieceSize * @ForestTreeFrequency) = 0 AND 
	CAST(Z AS INT) % (@TerrainPieceSize * @ForestTreeFrequency) = 0


SELECT @@ROWCOUNT

ROLLBACK TRANSACTION
/* Dead trees 
65811
65812
65813
*/

/* Live trees 
65814
65815
65816
*/

/* Terrain Types
1	Plains		Flat, reasonably well irrigated terrain.
2	Hills		Hilly
3	Mountains		Mountainous
4	Forest/Hills		Wooded Hills
5	Swamp		Low lying land
6	City		City/indoor
7	Road		The open road
8	Water		Water
9	Forest		Forest
10	Ruins		Ruins
*/