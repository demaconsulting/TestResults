# Security Policy

## Supported Versions

We release patches for security vulnerabilities for the following versions:

| Version | Supported          |
| ------- | ------------------ |
| Latest  | :white_check_mark: |
| < Latest| :x:                |

We recommend always using the latest version of the TestResults library to ensure you have the most recent security
updates and bug fixes.

## Reporting a Vulnerability

The TestResults team takes security vulnerabilities seriously. We appreciate your efforts to responsibly disclose your
findings.

### How to Report a Security Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report them using one of the following methods:

1. **GitHub Security Advisories** (Preferred)
   - Go to the [Security tab](https://github.com/demaconsulting/TestResults/security)
   - Click on "Report a vulnerability"
   - Fill out the form with details about the vulnerability

2. **Direct Contact**
   - Open a private security advisory on GitHub
   - Contact the project maintainers directly through GitHub

### What to Include in Your Report

Please include the following information in your report:

- **Description**: A clear description of the vulnerability
- **Impact**: What kind of vulnerability is it and what is the potential impact?
- **Location**: Where in the codebase the vulnerability exists
- **Reproduction Steps**: Detailed steps to reproduce the vulnerability
- **Proof of Concept**: If possible, provide a proof-of-concept or exploit code
- **Suggested Fix**: If you have a suggestion for how to fix the vulnerability, please include it
- **Your Details**: Your name and contact information (optional, but helpful for follow-up questions)

### What to Expect

After you submit a vulnerability report:

1. **Acknowledgment**: We will acknowledge receipt of your vulnerability report within 3 business days
2. **Investigation**: We will investigate and validate the vulnerability
3. **Updates**: We will keep you informed about the progress of addressing the vulnerability
4. **Resolution**: We will work on a fix and coordinate the release with you
5. **Credit**: We will credit you in the security advisory (unless you prefer to remain anonymous)

### Disclosure Policy

- We ask that you give us reasonable time to address the vulnerability before any public disclosure
- We will work with you to understand the severity and impact of the vulnerability
- We will coordinate the release of security patches and advisories with you
- We will publicly acknowledge your responsible disclosure (if you wish to be credited)

## Security Measures

The TestResults library implements several security best practices:

### Code Security

- **No External Dependencies**: The library has zero runtime dependencies, minimizing the attack surface
- **Input Validation**: All public APIs validate their inputs
- **No File Operations**: The library does not perform file I/O operations internally (caller controls file operations)
- **No Network Operations**: The library does not make any network calls
- **Static Analysis**: Code is analyzed with Microsoft.CodeAnalysis.NetAnalyzers
- **Warnings as Errors**: All compiler warnings are treated as errors

### Build Security

- **Continuous Integration**: All code changes go through automated CI/CD pipelines
- **SonarCloud Analysis**: Code quality and security are analyzed by SonarCloud
- **Code Coverage**: Unit tests provide comprehensive code coverage
- **SBOM Generation**: Software Bill of Materials is generated for all releases
- **Dependency Scanning**: Dependencies are regularly scanned for known vulnerabilities

### Development Security

- **Code Review**: All changes require code review before merging
- **Branch Protection**: The main branch is protected and requires passing CI checks
- **Signed Commits**: We encourage signed commits for verification
- **Least Privilege**: Development follows the principle of least privilege

## Known Vulnerabilities

We maintain transparency about known security issues:

- No known vulnerabilities at this time

Historical security advisories can be found in the
[GitHub Security Advisories](https://github.com/demaconsulting/TestResults/security/advisories) section.

## Security Best Practices for Users

When using the TestResults library:

1. **Keep Updated**: Always use the latest version of the library
2. **Validate Input**: If accepting TRX file paths from users, validate and sanitize them
3. **File Permissions**: When writing TRX files, use appropriate file permissions
4. **Error Handling**: Implement proper error handling when using the library
5. **Code Review**: Review any code that uses this library as part of your security practices

## Scope

This security policy applies to:

- The TestResults library source code
- Official NuGet packages published by DEMA Consulting
- GitHub repository and infrastructure

This policy does not cover:

- Third-party applications that use the TestResults library
- Forks of the repository not maintained by DEMA Consulting
- Issues in dependencies (report those to the respective projects)

## Contact

For any questions about this security policy, please open an issue in the GitHub repository or contact the maintainers
directly.

## Attribution

This security policy is based on security policy best practices and adapted for the TestResults project.
