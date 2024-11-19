DELIMITER //

create procedure CreateUser(
 in p_username varchar(50),
 in p_password varchar(50),
 in p_role varchar(50),
 in p_firstname varchar(50),
 in p_lastname varchar(50),
 in p_email varchar(50),
 in p_cell varchar(50),
 in p_filetype varchar(50)
)
begin
declare v_last_user int;

insert into User(username, password, role)
values(p_username,p_password,p_role);

set v_last_user = last_insert_id();

insert into userdetails(firstname, lastname, email, cell)
values(p_firstname,p_lastname,p_email,p_cell);

end//

DELIMITER ;
