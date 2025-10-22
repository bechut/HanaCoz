DROP TABLE IF EXISTS item

CREATE TABLE IF NOT EXISTS item (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT NOT NULL UNIQUE,
    name TEXT,
    type TEXT,
    region_x INTEGER,
    region_y INTEGER,
    region_w INTEGER,
    region_h INTEGER,
    description TEXT,
    CHECK (type IN ('outfit', 'accessory', 'hairstyle'))
);

DROP TABLE IF EXISTS storage_item

CREATE TABLE IF NOT EXISTS storage_item (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    item_id INTEGER NOT NULL,
    storage_code TEXT NOT NULL,
    item_order INTEGER,
    FOREIGN KEY (item_id) REFERENCES item(id)
);

drop table player
drop table player_item

CREATE TABLE IF NOT EXISTS player (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT,
    pos_x INTEGER,
    pos_y INTEGER,
    outfit_id INTEGER,
    FOREIGN KEY (outfit_id) REFERENCES item(id)
);


INSERT INTO storage_item (item_id, storage_code, item_order) VALUES
(1, '001', 0),
(3, '001', 1),
(4, '001', 2);

SELECT
    T1.id, T1.storage_code, T2.name, T2.description, 
    T2.code, T2.type, T2.region_x,T2.region_y,T2.region_w, T2.region_h
FROM
    storage_item AS T1
        INNER JOIN
    item AS T2 ON T1.item_id = T2.id
WHERE
    T1.storage_code = '001';

SELECT
    p.id AS player_id,
    p.code AS player_code,
    p.pos_x,
    p.pos_y,
    pi.id AS player_item_id,
    pi.item_id,
    pi.equipped,
    i.code,
    i.name,
    i.description
FROM player AS p
         INNER JOIN player_item AS pi ON pi.player_id = p.id
         INNER JOIN item AS i ON pi.item_id = i.id
WHERE p.code = 'main';