# Repositories

Unity's Collaboration function is mainly used for easily adding assets and syncing or collaborating with team members.
However, Project KMR will use Github as the main repository where project development progress is tracked and shared.
In short,

Github:

- Used for tracking milestones
- Commit major developments and improvements to the game to this repo
- Use branches for versioning and **only** merge to master when the
  version is completed. (In other words, branch versions should act as
  Alpha builds while the master is the late Beta or the release build)
- Used for submitting issues, project boards, etc.

Unity Collaboration:

- Used for uploading assets like music, art, graphics, etc. (This can
  be done via Github, but Collaboration is easier to use)
- Used for quickly downloading projects from Unity Hub
- Collaborate with team members
- Incremental and procedural updates to the current build
- Does not distinguish between Alpha or Beta versions
- Publishing changes to the project should match the commits to Github

Overall for developers, I recommend using Github for starting your development and committing your progress.
For collaborating with other developers, you can use a VS Code extension called [Live Share](https://marketplace.visualstudio.com/items?itemName=MS-vsliveshare.vsliveshare).

# Github Guide

Because Github provides the developer with massive features, we will write a separate section that defines how to use each features that is required.

## Branches and Forks

When implementing a new feature for most projects, you would create a branch that is only for that feature.
When the development is done you would open a pull request and merge it with the master.
However, we will doing things a bit differently as game development involves many moving parts that rely on each other.
For KMR, branches will be the WIP versions and nested branches are the developing feature or mechanism for that version.

Let's say there's a branch named `v-0.1.3a/ai_hearing_sensor`\*
In here, `v-0.1.3a` is the version for the build and `ai_hearing_sensor` is the current in-development feature.

\*Note: the actual name of the branch does not have `/` as Github does not allow this. So please make sure that the feature branch is derived from the version branch.

Once the implementation of the feature is done and you tested for errors, you should open a Pull Request that merges the feature branch to the parent version branch.
using the above example, when you finishing implementing the hearing sensor for the AI, you should open a PR that merges `v-0.1.3a/ai_hearing_sensor` to `v-0.1.3a` and delete the branch `v-0.1.3a/ai_hearing_sensor` when deemed appropriate.
However, even if a version is completed, you should not delete a version branch even after a merge.
But a nested child branch should always be removed after a PR.

Another method of collaborative work is, instead of making branches, you can **Fork** the repo.
implement the feature that you want to develop, commit to the **version branch** (`v-0.1.3a` in the above example).
Once the development is finished, merge the fork to the original repo and merge it to the version branch and not the master.

**Never commit directly to the master!**
However, you are allowed to commit directly to any branches or the master when there is a very minor error that you know how to fix without requiring any tests afterwards.

## Master

The master branch of a project repo should always be the latest finished build.
There should be no obvious bugs in the master branch as the master is assumed to be a Good-to-Show build if not the release build.

## Pull Request Format

In general, there are two types of pull requests,

- Feature Branch to Version Branch (or forked version branch to original version branch)
- Version Branch to Master Branch

The first, feature branch to version branch PR will be the most common PR during active development.
The description for this PR is not that important (but having a comprehensive description of what was added is always helpful).
Instead the title of the PR is very important as that is the main indicator of what was added.
And putting importance in the title of the branch and the PR allows the developers to implement one feature at a time rather than doing multiple work at the same time.
For example, if the name of the branch is `v-0.1.4a/throw_objects`, the title of the PR for this should be "Implemented throwable objects and throwing mechanics".
This may be confusing (if so you are free to propose a better solution) but to put it simply, the title should say the major additions for the feature, _something that can be written in the game update log_.

The second type, version branch to master branch PR is the opposite of the feature branch to merge branch, as the title should only say what version the master is going to be updated to.
Instead the description should contain the full change log (version branch commits) for each features added.
It should also summarize what the update is doing.
This is important because the _description of the PR will be part of the `README.md` file for the master branch_.

## Typical Workflow

A typical workflow starting from adding a feature to updating the game's version will be something like the following.

1. Create a version branch (`v-0.2.0a`)
2. Create a feature branch (`v-0.2.0a/spell_base`)
3. Develop the feature and test that it works, commit work when done
4. Open a PR and merge feature branch to the version branch (`v-0.2.0a/spell_base -> v-0.2.0a`)
5. Create another feature branch when needed, repeat step 3
6. Open a PR and merge version branch to master (`v-0.2.0a -> master`)
