-- Create content users table
CREATE TABLE users (
                       id SERIAL PRIMARY KEY,
                       name VARCHAR(50) NOT NULL,
                       function VARCHAR(50) NOT NULL
);

-- Create document table
CREATE TABLE documents (
                           id SERIAL PRIMARY KEY,
                           name VARCHAR(100) NOT NULL,
                           content TEXT NOT NULL,
                           owner_user_id INT NOT NULL,
                           FOREIGN KEY (owner_user_id) REFERENCES users(id)
);

-- Create authorized users table
CREATE TABLE authorized_users (
                                  document_id INT NOT NULL,
                                  user_id INT NOT NULL,
                                  PRIMARY KEY (document_id, user_id),
                                  FOREIGN KEY (document_id) REFERENCES documents(id),
                                  FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Create document tags table
CREATE TABLE document_tags (
                               document_id INT NOT NULL,
                               tag VARCHAR(50) NOT NULL,
                               PRIMARY KEY (document_id, tag),
                               FOREIGN KEY (document_id) REFERENCES documents(id)
);

-- Create document revisions table
CREATE TABLE document_revisions (
                                    id SERIAL PRIMARY KEY,
                                    document_id INT NOT NULL,
                                    revision_number INT NOT NULL,
                                    revision_date DATE NOT NULL,
                                    content TEXT NOT NULL,
                                    FOREIGN KEY (document_id) REFERENCES documents(id)
);

-- Create document categories table
CREATE TABLE document_categories (
                                     document_id INT NOT NULL,
                                     category VARCHAR(50) NOT NULL,
                                     PRIMARY KEY (document_id, category),
                                     FOREIGN KEY (document_id) REFERENCES documents(id)
);

commit;