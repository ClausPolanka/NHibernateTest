CREATE TABLE Person ( 
	nummer		INTEGER 	PRIMARY KEY, 
	name		VARCHAR(30), 
	geburtsdatum DATE
);

CREATE TABLE Mechaniker ( 
	nummer		INTEGER 	PRIMARY KEY REFERENCES Person
);

CREATE TABLE Lehrer ( 
	nummer		INTEGER 	PRIMARY KEY REFERENCES Person,
	erfahrung	INTEGER
);

CREATE TABLE Schueler ( 
	nummer		INTEGER 	PRIMARY KEY REFERENCES Person
);

CREATE TABLE Flugzeug (
	id		INTEGER		PRIMARY KEY,
	name		VARCHAR(30),
	modell		VARCHAR(30),
	hersteller	VARCHAR(30),
	spannweite	REAL
);

CREATE TABLE Motorflugzeug (
	id		INTEGER 	PRIMARY KEY REFERENCES Flugzeug,
	leistung	INTEGER
);

CREATE TABLE Segelflugzeug (
	id		INTEGER 	PRIMARY KEY REFERENCES Flugzeug
);

CREATE TABLE besitzt (
	nummer		INTEGER 	REFERENCES Lehrer,
	id		INTEGER		PRIMARY KEY REFERENCES Flugzeug
);

CREATE TABLE Hangar (
	name		VARCHAR(30)	PRIMARY KEY,
	flaeche		INTEGER
);

CREATE TABLE verwaltet (
	name		VARCHAR(30)	REFERENCES Hangar,
	nummer		INTEGER		REFERENCES Mechaniker,
	PRIMARY KEY (name, nummer)
);

CREATE TABLE abgestellt (
	name		VARCHAR(30)	REFERENCES Hangar,
	id		INTEGER		REFERENCES Flugzeug,
	PRIMARY KEY (id)
);

CREATE TABLE Flugeinheit ( 
	snummer		INTEGER		REFERENCES Schueler (nummer), 
	datum		DATE,
	dauer		INTEGER,
	lehrinhalt	VARCHAR(30),
	lnummer		INTEGER		REFERENCES Lehrer (nummer),
	id		INTEGER		REFERENCES Flugzeug,
	PRIMARY KEY (snummer, datum)
);

CREATE TABLE Wettbewerb (
	datum		DATE,
	name		VARCHAR(30),
	ort		VARCHAR(30),
	land		VARCHAR(30),
	PRIMARY KEY (datum, name)
);

CREATE TABLE fliegt (
	nummer		INTEGER REFERENCES Lehrer,
	id		INTEGER REFERENCES Flugzeug,
	datum		DATE,
	name		VARCHAR(30),
	platz		INTEGER,
	FOREIGN KEY (datum, name) REFERENCES Wettbewerb (datum, name),
	PRIMARY KEY (nummer, id, datum, name)
);
 
