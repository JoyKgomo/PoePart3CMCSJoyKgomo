DELIMITER //

Create procedure submitClaim(
in p_dateclaimed date,
in p_hours int,
in p_rate double,
in p_amountdue double,
in p_additionaldetails varchar(50),
in p_document longblob,
in p_claim_id int
)

begin
declare v_last_user int;

START TRANSACTION;

insert into Claim(claimdate, hours, rate)
values (p_dateclaimed, p_hours, p_rate);

set v_last_user = last_insert_id();
INSERT INTO document (claim_id, document)
VALUES (p_claim_id, p_document);

COMMIT;

rollback;

end//

DELIMITER ;
