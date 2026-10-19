CREATE TABLE robot_devices (
    device_id CHAR(36) PRIMARY KEY,
    name VARCHAR(120) NOT NULL,
    mode VARCHAR(32) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE telemetry (
    telemetry_id BIGINT AUTO_INCREMENT PRIMARY KEY,
    device_id CHAR(36) NOT NULL,
    joint_id VARCHAR(120) NOT NULL,
    position DOUBLE NOT NULL,
    velocity DOUBLE NOT NULL,
    acceleration DOUBLE NOT NULL,
    recorded_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_telemetry_device_time (device_id, recorded_at)
);

CREATE TABLE safety_events (
    event_id BIGINT AUTO_INCREMENT PRIMARY KEY,
    device_id CHAR(36) NOT NULL,
    event_type VARCHAR(64) NOT NULL,
    message VARCHAR(500) NOT NULL,
    recorded_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
