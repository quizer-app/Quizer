﻿namespace Quizer.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password,
    bool RememberMe
);
