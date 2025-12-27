# GitHub AI Agents

This directory contains custom GitHub Copilot AI agent configurations for the TestResults project.
These agents have specialized roles and expertise to help maintain and improve the project.

## Available Agents

### 1. Documentation Writer (`documentation-writer.yml`)

**Role**: Expert technical writer for documentation maintenance

**Specialties**:

- Maintaining README, ARCHITECTURE, and other markdown files
- Writing XML documentation comments for C# APIs
- Ensuring documentation accuracy and clarity
- Following markdown and spelling standards

**When to Use**:

- Adding or updating documentation
- Improving code comments
- Creating usage examples
- Fixing documentation issues

**How to Invoke**: `@copilot[documentation-writer]`

### 2. Software Quality Enforcer (`software-quality-enforcer.yml`)

**Role**: Code quality specialist ensuring high standards

**Specialties**:

- Enforcing testing standards and code coverage
- Running static analysis and linting
- Code review and quality gates
- Ensuring zero-warning builds

**When to Use**:

- Reviewing code changes
- Improving test coverage
- Fixing quality issues
- Enforcing coding standards

**How to Invoke**: `@copilot[software-quality-enforcer]`

### 3. Project Maintainer (`project-maintainer.yml`)

**Role**: Project maintenance and improvement specialist

**Specialties**:

- Managing dependencies and Dependabot PRs
- Triaging and organizing issues
- Identifying improvement opportunities
- Planning enhancements and releases
- Weekly maintenance tasks

**When to Use**:

- Weekly project health checks
- Issue triage and prioritization
- Planning enhancements
- Dependency updates
- Release preparation

**How to Invoke**: `@copilot[project-maintainer]`

## Weekly Maintenance

The project includes a weekly maintenance workflow (`.github/workflows/weekly-maintenance.yml`)
that automatically creates maintenance issues every Monday. The Project Maintainer agent can
be assigned to these issues to perform regular project health checks.

**To enable**: The workflow is already configured and will run automatically.
**To trigger manually**: Use the "Run workflow" button in the GitHub Actions tab.

## Using the Agents

### In Issues and Pull Requests

You can invoke agents by mentioning them in comments:

```markdown
@copilot[documentation-writer] Please update the README with the new feature examples.
```

```markdown
@copilot[software-quality-enforcer] Review this PR for quality standards.
```

```markdown
@copilot[project-maintainer] Please triage these recent issues and suggest priorities.
```

### In Copilot Chat

You can also interact with agents directly in GitHub Copilot Chat by selecting
the appropriate agent from the agent picker.

## Agent Collaboration

The agents are designed to work together:

- **Documentation Writer** + **Software Quality Enforcer**: Ensure docs match implementation
- **Software Quality Enforcer** + **Project Maintainer**: Coordinate on quality improvements
- **Project Maintainer** + **Documentation Writer**: Keep project docs current

## Best Practices

1. **Choose the Right Agent**: Select the agent whose expertise matches your task
2. **Be Specific**: Provide clear context and instructions when invoking agents
3. **Review Output**: Always review agent suggestions before applying changes
4. **Iterate**: Agents can iterate on feedback - provide guidance if initial output needs adjustment
5. **Combine Agents**: Use multiple agents for complex tasks requiring different expertise

## Agent Limitations

- Agents have access to repository contents and history
- Agents follow the instructions in their configuration files
- Agents respect project boundaries (won't add dependencies, make breaking changes, etc.)
- Agents work within GitHub's rate limits and API constraints

## Modifying Agents

To modify an agent's behavior:

1. Edit the corresponding `.yml` file in this directory
2. Update the `instructions` section with new guidance
3. Test the changes by invoking the agent
4. Document significant changes in this README

## Feedback

If you have suggestions for improving the agents or want to propose new agents,
please open an issue with the label `agent-improvement`.

## Project Guidelines

All agents follow the project's established guidelines:

- Code style per `.editorconfig`
- Testing standards (MSTest, >80% coverage)
- No external runtime dependencies
- Zero-warning builds
- Quality gates (SonarCloud, static analysis)
- Documentation standards (markdownlint, cspell)

See `AGENTS.md` in the root directory for complete AI agent guidelines.
