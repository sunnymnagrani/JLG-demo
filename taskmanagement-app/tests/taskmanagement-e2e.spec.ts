import { test, expect } from '@playwright/test';

test.describe('Task Management E2E Flow', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:4200'); 
  });

  test('should create a new task and filter it successfully', async ({ page }) => {
    await expect(page.locator('h3:has-text("Task List")')).toBeVisible();

    //Open the Add Task Modal
    await page.click('button:has-text("Add Task")');
    
    // Verify the modal heading changed to "Add Task"
    await expect(page.locator('.modal-title')).toHaveText('Add Task');

    const uniqueTitle = `Playwright Test Task ${Date.now()}`;
    await page.fill('input[formControlName="taskTitle"]', uniqueTitle);
    await page.fill('input[formControlName="taskDesc"]', 'This task was generated automatically.');
    
    await page.check('input[formControlName="taskStatus"]');

    page.once('dialog', async dialog => {
      expect(dialog.message()).toContain('Task Added Successfully');
      await dialog.accept();
    });
    
    await page.click('button[type="submit"]:has-text("Save")');

    // Verify the new task appeared in the main table layout
    const taskRow = page.locator('table tbody tr').filter({ hasText: uniqueTitle });
    await expect(taskRow).toBeVisible();
    
    // Verify the badge text reads 'Active'
    await expect(taskRow.locator('.badge')).toHaveText('Active');

    // Test the Custom Status Dropdown Filter functionality
    const filterDropdown = page.locator('select');
    
    // Switch filter selection to "Completed"
    await filterDropdown.selectOption('Completed');
    await expect(taskRow).not.toBeVisible();

    // Switch filter selection back to "Active"
    await filterDropdown.selectOption('Active');
    await expect(taskRow).toBeVisible();
  });
});
